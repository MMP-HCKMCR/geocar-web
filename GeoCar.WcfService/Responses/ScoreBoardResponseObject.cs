using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GeoCar.WcfService
{
    public class ScoreBoardResponseObject
    {
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int Score { get; set; }
    }
}