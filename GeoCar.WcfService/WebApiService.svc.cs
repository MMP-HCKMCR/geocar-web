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
            var loginResult = UserRepository.LogInUser(request.EmailAddress, request.UserPassword);

            if (loginResult.ErrorNumber == 0)
            {
                return new LoginResponse
                {
                    ErrorId = 0,
                    SessionId = loginResult.SessionId,
                    Success = true,
                    UserId = loginResult.UserId
                };
            }

            return new LoginResponse
            {
                ErrorId = loginResult.ErrorNumber,
                SessionId = string.Empty,
                Success = false
            };
        }

        public RegisterTagResponse RegisterTag(RegisterTagRequest request)
        {
            var tag = TagRepository.RetrieveTag(request.BeaconId,
                                                request.BeaconMajorVersion,
                                                request.BeaconMinorVersion);

            var tagType = TagTypeRepository.RetrieveTagType(tag.TagTypeId);

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
                ErrorId = 0
            };
        }
    }
}
