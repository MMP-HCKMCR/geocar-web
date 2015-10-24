using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GeoCar.WcfService.Responses
{
    public class TransactionResponse
    {
        [DataMember]
        public int TransactionId { set; get; }
        [DataMember]
        public int UserId { set; get; }
        [DataMember]
        public int Points { set; get; }
        [DataMember]
        public DateTime TimeCaptured { set; get; }
        [DataMember]
        public string TransactionType { set; get; }
    }
}