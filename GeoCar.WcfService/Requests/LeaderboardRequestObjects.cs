using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Requests
{
    public class LeaderboardRequest
    {
        [DataMember]
        public Guid SessionId { get; set; }
    }
}