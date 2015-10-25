using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GeoCar.Model
{
    public class Hire
    {
        public int HireUserId { get; set; }
        public int UserId { get; set; }
        public string BookingReference { get; set; }
        public DateTime HireStartDate { get; set; }
        public DateTime? HireEndDate { get; set; }
        public int StartMileage { get; set; }
        public int EndMileage { get; set; }

        public User HireUser { get; set; }

        public static Hire FromDataRow(DataRow dr)
        {
            return new Hire
            {
                HireUserId = dr.Field<int>("HireUserId"),
                UserId = dr.Field<int>("UserId"),
                BookingReference = dr.Field<string>("BookingReference"),
                HireStartDate = dr.Field<DateTime>("HireStartDate"),
                HireEndDate = dr.Field<DateTime?>("HireEndDate"),
                StartMileage = dr.Field<int>("StartMileage"),
                EndMileage = dr.Field<int>("EndMileage")
            };
        }
    }
}
