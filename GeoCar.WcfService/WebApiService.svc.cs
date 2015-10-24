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

            //TODO: Achievements

            var transaction = TransactionRepository.CreateTransactionForUserAndTag(user.UserId, pointsScored, tag.TagId, TransactionType.Tag);

            user.Score = user.Score + pointsScored;

            user = UserRepository.UpdateUser(user);

            return new RegisterTagResponse
            {
                PointsScored = pointsScored,
                NewPointsTotal = user.Score,
                UsablePoints = TransactionRepository.GetUsersUsablePoints(user.UserId),
                Achievement = String.Empty,
                Top10 = new LeaderboardResponseObject[0],
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
                TotalPoints = user.UserId,
                UsablePoints = TransactionRepository.GetUsersUsablePoints(user.UserId),
                Last5Transactions = MapOutTransactions(TransactionRepository.RetrieveXTransactionsForUser(5, user.UserId))
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
                Achievement = String.Empty,
                Top10 = new LeaderboardResponseObject[0],
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


        #endregion

        //public LeaderboardResponseObject GetLeaderboard(LeaderboardRequestObject request)
        //{
        //    var session = SessionRepository.RetrieveSession(request.SessionId);
        //    if (session == null)
        //    {
        //        return new LeaderboardResponseObject
        //        {
        //            Success = false,
        //            ErrorMessage = "InvalidSessionId",
        //        };
        //    }
        //}
    }
}
