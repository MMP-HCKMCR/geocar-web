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
            var top10 = UserRepository.RetrieveTopLeaderboard(10, user.UserId);

            return new RegisterTagResponse
            {
                PointsScored = pointsScored,
                NewPointsTotal = user.Score,
                UsablePoints = 0,
                Achievement = string.Empty,
                Top10 = top10.Select(LeaderboardResponseEntry.FromModel).ToArray(),
                Ranking = 0,
                LockoutTime = tagType.LockoutTimePeriod,
                Success = true,
                ErrorMessage = string.Empty
            };
        }

        private static RegisterTagResponse CreateFailedTagResponse(string errorMesssage)
        {
            return new RegisterTagResponse
            {
                PointsScored = 0,
                NewPointsTotal = 0,
                UsablePoints = 0,
                Achievement = string.Empty,
                Top10 = new Responses.LeaderboardResponseEntry[0],
                Ranking = 0,
                LockoutTime = 0,
                Success = false,
                ErrorMessage = errorMesssage
            };
        }

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
