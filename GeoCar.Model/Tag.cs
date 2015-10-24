using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCar.Model
{
    public class Tag
    {
        public int TagId { get; set; }
        public string UUID { get; set; }
        public int MajorNumber { get; set; }
        public int MinorNumber { get; set; }
        public bool Active { get; set; }
        public int AdditionalPoints { get; set; }
        public int TagTypeId { get; set; }
    }
}
