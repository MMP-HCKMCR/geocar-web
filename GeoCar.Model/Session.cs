
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCar.Model
{
    public class Session : ModelBase
    {
        public string SessionId { set; get; }
        public int UserId { set; get; }
    }
}
