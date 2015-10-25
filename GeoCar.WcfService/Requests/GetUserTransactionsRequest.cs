using System;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Requests
{
    public class GetUserTransactionsRequest
    {
        [DataMember]
        public Guid SessionId { set; get; }
    }
}