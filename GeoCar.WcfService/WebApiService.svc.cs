using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

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
            var response = new LoginResponse();
            
            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                response.Errors.Add("Parameter 'EmailAddress' is missing");
                response.Success = false;
            }
            if (string.IsNullOrEmpty(request.UserPassword))
            {
                response.Errors.Add("Parameter 'UserPassword' is missing");
                response.Success = false;
            }

            if (response.Errors.Count > 0)
            {
                return response;
            }

            // TODO: un-stub

            response.Success = true;
            response.SessionID = "abcd1234abcd1234efef5678efef5678";
            return response;
        }
    }
}
