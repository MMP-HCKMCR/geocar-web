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
            var userRepository = new UserRepository();
            var loginResult = userRepository.LogInUser(request.EmailAddress, request.UserPassword);

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
    }
}
