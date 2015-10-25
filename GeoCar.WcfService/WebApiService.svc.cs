using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GeoCar.Database;
using GeoCar.Model;
using GeoCar.WcfService.Requests;
using GeoCar.WcfService.Responses;

namespace GeoCar.WcfService
{
    public class WebApiService : IWebApiService
    {
        public MarcoResponse Marco()
        {
            return new MarcoResponse()
            {
                Success = true,
                Marco = "polo"
            };
        }

        public LoginResponse Login(LoginRequest request)
        {
            var session = UserRepository.LogInUser(request.EmailAddress, request.UserPassword);

            if (session != null)
            {
                return new LoginResponse
                {
                    SessionId = session.SessionId,
                    Success = true,
                    UserId = session.UserId
                };
            }

            return new LoginResponse
            {
                SessionId = string.Empty,
                Success = false,
                ErrorMessage = "LoginIncorrect"
            };
        }

        public ApiResult Logout(LogoutRequest request)
        {
            var sessionWasValid = SessionRepository.InvalidateSession(request.SessionId);

            return new ApiResult
            {
                Success = true,
                ErrorMessage = (sessionWasValid ? string.Empty : "InvalidSessionId")
            };
        }

        public RegisterTagResponse RegisterTag(RegisterTagRequest request)
        {
            var tag = TagRepository.RetrieveTag(request.BeaconId,
                                                request.BeaconMajorVersion,
                                                request.BeaconMinorVersion);

            if (tag == null || !tag.Active)
            {
                return CreateFailedTagResponse("Tag not active");
            }

            var user = UserRepository.RetrieveUser(request.SessionId);

            if (user == null)
            {
                return CreateFailedTagResponse("Session not Valid");
            }

            var tagType = TagTypeRepository.RetrieveTagType(tag.TagTypeId);

            if (tagType == null)
            {
                return CreateFailedTagResponse("Invalid Tag Type");
            }

            if (TagRepository.CheckTagIsNotWithinTimeout(user.UserId, tag.TagId, tagType.LockoutTimePeriod))
            {
                return CreateFailedTagResponse("Tag seen within Timeout");
            }

            var pointsScored = tagType.Points + tag.AdditionalPoints;

            var achievements = AchievementRepository.RetrieveAvailableAchievements(user.UserId, tag.TagId);

            var achievementNames = new List<string>();

            if (achievements != null)
            {
                foreach (var achievement in achievements)
                {
                    achievementNames.Add(achievement.AchievementName);
                    pointsScored = pointsScored + achievement.RewardPoints;
                }
            }

            TransactionRepository.CreateTransactionForUserAndTag(user.UserId, pointsScored, tag.TagId, TransactionType.Tag);

            user.Score = user.Score + pointsScored;

            user = UserRepository.UpdateUser(user);

            var top = UserRepository.RetrieveTopLeaderboard(3, user.UserId);

            return new RegisterTagResponse
            {
                PointsScored = pointsScored,
                NewPointsTotal = user.Score,
                UsablePoints = TransactionRepository.GetUsersUsablePoints(user.UserId),
                Achievements = achievementNames.ToArray(),
                Top10 = top.Select(LeaderboardResponseEntry.FromModel).ToArray(),
                Ranking = 0,
                LockoutTime = tagType.LockoutTimePeriod,
                Success = true,
                ErrorMessage = string.Empty
            };
        }

        public UserInfoResponse UserInfo(UserInfoRequest request)
        {
            var user = UserRepository.RetrieveUser(request.SessionId);

            if (user == null)
            {
                return new UserInfoResponse
                {
                    FirstName = string.Empty,
                    Surname = string.Empty,
                    Email = string.Empty,
                    TotalPoints = 0,
                    UsablePoints = 0,
                    Last5Transactions = new TransactionResponse[0],
                    Success = false,
                    ErrorMessage = "Invalid Session Details"
                };
            }

            return new UserInfoResponse
            {
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                TotalPoints = user.Score,
                UsablePoints = TransactionRepository.GetUsersUsablePoints(user.UserId),
                Last5Transactions = MapOutTransactions(TransactionRepository.RetrieveXTransactionsForUser(5, user.UserId)),
                Success = true
            };
        }

        public GetAchievementResponse GetAchievements(GetAchievementRequest request)
        {
            var user = UserRepository.RetrieveUser(request.SessionId);

            if (user == null)
            {
                return new GetAchievementResponse
                {
                    Success = false,
                    ErrorMessage = "User not Found"
                };
            }

            var currentAchievements = AchievementRepository.RetrieveUserAchievements(user.UserId);
            var outstandingAchievements = AchievementRepository.RetrieveOutstandingUserAchievements(user.UserId);

            return new GetAchievementResponse
            {
                Success = true,
                UsersAchievements = MapOutAchievements(currentAchievements),
                RemainingAchievements = MapOutAchievements(outstandingAchievements)
            };
        }

        public GetUserTransactionsResponse GetUserTransactions(GetUserTransactionsRequest request)
        {
            var user = UserRepository.RetrieveUser(request.SessionId);

            if (user == null)
            {
                return new GetUserTransactionsResponse
                {
                    Success = false,
                    ErrorMessage = "User not Found"
                };
            }

            return new GetUserTransactionsResponse
            {
                Success = true,
                TransactionDetails = MapOutTransactions(TransactionRepository.RetrieveXTransactionsForUser(20, user.UserId)),
            };
        }

        public RegisterUserResponse RegisterUser(RegisterUserRequest request)
        {
            var user = UserRepository.RetrieveUser(request.EmailAddress, request.BookingReference);

            if (user == null)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    ErrorMessage = "User not Found"
                };
            }

            user.Password = request.Password;

            user = UserRepository.UpdateUser(user);

            var session = SessionRepository.CreateSession(user.UserId);

            if (session == null)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    ErrorMessage = "Session could not be Created"
                };
            }

            return new RegisterUserResponse
            {
                Success = true,
                SessionId = session.SessionId
            };
        }

        #region Private Support Methods
        private RegisterTagResponse CreateFailedTagResponse(string errorMesssage)
        {
            return new RegisterTagResponse
            {
                PointsScored = 0,
                NewPointsTotal = 0,
                UsablePoints = 0,
                Achievements = new string[0],
                Top10 = new Responses.LeaderboardResponseEntry[0],
                Ranking = 0,
                LockoutTime = 0,
                Success = false,
                ErrorMessage = errorMesssage
            };
        }

        private TransactionResponse[] MapOutTransactions(List<Transaction> transactions)
        {
            return transactions.Select(transaction => new TransactionResponse
            {
                TransactionId = transaction.TransactionId,
                UserId = transaction.UserId,
                Points = transaction.Points,
                TimeCaptured = transaction.TimeCaptured,
                TransactionType = MapOutTransactionType(transaction.TransactionTypeId)
            }).ToArray();
        }

        private string MapOutTransactionType(int transactionTypeId)
        {
            var result = string.Empty;

            switch (transactionTypeId)
            {
                case 1:
                    result = "Tag";
                    break;
                case 2:
                    result = "Milage";
                    break;
                case 3:
                    result = "Usage";
                    break;
            }

            return result;
        }

        private AchievementResponse[] MapOutAchievements(List<Achievement> achievements)
        {
            return achievements.Select(achievement => MapOutAchievement(achievement)).ToArray();
        }

        private AchievementResponse MapOutAchievement(Achievement achievement)
        {
            return new AchievementResponse
            {
                AchievementName = achievement.AchievementName,
                AchievementDescription = achievement.AchievementDescription
            };

        }

        #endregion

        public LeaderboardResponse GetLeaderboard(LeaderboardRequest request)
        {
            var session = SessionRepository.RetrieveSession(request.SessionId);
            if (session == null)
            {
                return new LeaderboardResponse
                {
                    Success = false,
                    ErrorMessage = "InvalidSessionId",
                };
            }

            var result = new LeaderboardResponse();

            var top = UserRepository.RetrieveTopLeaderboard(10, session.UserId);
            result.Top10 = top.Select(LeaderboardResponseEntry.FromModel).ToArray();

            var contenders = UserRepository.RetrieveLocalLeaderboard(session.UserId, 2);
            result.Contenders = contenders.Select(LeaderboardResponseEntry.FromModel).ToArray();
            result.Ranking = contenders.Where(c => c.IsCurrentUser).Select(c => c.Position).First();

            return result;
        }
    }
}
