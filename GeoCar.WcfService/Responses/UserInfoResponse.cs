using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class UserInfoResponse : ApiResult
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Surname { set; get; }

        [DataMember]
        public string Email { set; get; }

        [DataMember]
        public int TotalPoints { set; get; }

        [DataMember]
        public int UsablePoints { set; get; }

        [DataMember]
        public TransactionResponse[] Last5Transactions { set; get; }
    }
}