using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class GetUserTransactionsResponse
    {
        [DataMember]
        public TransactionResponse[] TransactionDetails { set; get; }
    }
}