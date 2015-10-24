using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCar.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public DateTime TimeCaptured { get; set; }
        public int TransactionTypeId { set; get; }
    }
}
