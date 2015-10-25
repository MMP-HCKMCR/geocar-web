using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class GetUserTransactionsResponse : ApiResult
    {
        [DataMember]
        public TransactionResponse[] TransactionDetails { set; get; }
    }
}