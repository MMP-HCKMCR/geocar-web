using System;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Requests
{
    public class UserInfoRequest
    {
        [DataMember]
        public Guid SessionId { get; set; }
    }
}