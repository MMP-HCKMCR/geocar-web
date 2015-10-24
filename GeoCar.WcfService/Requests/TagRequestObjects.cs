using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GeoCar.WcfService.Requests
{
    public class RegisterTagRequest
    {
        [DataMember]
        public Guid SessionId { get; set; }

        [DataMember]
        public Guid BeaconId { get; set; }

        [DataMember]
        public int BeaconMajorVersion { get; set; }

        [DataMember]
        public int BeaconMinorVersion { get; set; }
    }
}