using System.Runtime.Serialization;

namespace GeoCar.WcfService.Requests
{
    public class RegisterUserRequest
    {
        [DataMember]
        public string EmailAddress { set; get; }
        [DataMember]
        public string Password { set; get; }
        [DataMember]
        public string BookingReference { set; get; }
    }
}