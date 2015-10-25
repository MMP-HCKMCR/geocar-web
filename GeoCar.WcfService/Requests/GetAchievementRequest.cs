using System;
using System.Runtime.Serialization;
using GeoCar.WcfService.Responses;

namespace GeoCar.WcfService.Requests
{
    public class GetAchievementRequest : ApiResult
    {
        [DataMember]
        public Guid SessionId { get; set; }
    }
}