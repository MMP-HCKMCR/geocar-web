using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCar.Model
{
    public class TagType
    {
        public int TagTypeId { get; set; }
        public string TypeName { get; set; }
        public int Points { get; set; }
        public int LockoutTimePeriod { get; set; }
    }
}
