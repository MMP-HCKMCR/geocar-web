using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class LoginResponse : ApiResult
    {
        public LoginResponse() : base() { }

        [DataMember]
        public string SessionID { get; set; }
    }
}