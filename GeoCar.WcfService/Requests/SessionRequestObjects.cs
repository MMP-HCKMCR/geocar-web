using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Requests
{
    public class LoginRequest
    {
        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string UserPassword { get; set; }
    }
}