using System;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class RegisterUserResponse : ApiResult
    {
        [DataMember]
        public string SessionId { set; get; }
    }
}