using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GeoCar.Database;
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

            return new RegisterTagResponse
            {
                PointsScored = 0,
                NewPointsTotal = 0,
                UsablePoints = 0,
                Achievement = String.Empty,
                Top10 = new List<ScoreBoardResponseObject>(),
                Ranking = 0,
                PossitionMove = false,
                LockoutTime = 0,
                Success = true,
                ErrorMessage = string.Empty
            };
        }
    }
}
