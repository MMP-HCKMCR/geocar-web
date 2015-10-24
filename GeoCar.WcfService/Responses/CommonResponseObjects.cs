using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class ApiResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        public ApiResult()
        {
            Success = false;
            ErrorMessage = string.Empty;
        }
    }

    public class ApiFault : ApiResult
    {
        [DataMember]
        public string FaultType { get; set; }
    }

    public class MarcoResponse : ApiResult
    {
        public MarcoResponse() : base() { }

        [DataMember]
        public string Marco { get; set; }
    }
}