using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using GeoCar.Model;

namespace GeoCar.Database
{
    public static class HireRepository
    {
        public static IList<Hire> RetrieveAllHires()
        {
            var dataTable = DatabaseCommon.PerformAction("GetAllHires", null);
            return DatabaseCommon.ConvertTable(dataTable, Hire.FromDataRow);
        }

        public static Hire RetrieveSingleHire(string bookingReference)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("bookingReference", bookingReference)
            };

            var dataTable = DatabaseCommon.PerformAction("GetSingleHireForBookingReference", parameters);
            return DatabaseCommon.ConvertRow(dataTable, Hire.FromDataRow);
        }

        public static Hire RetrieveSingleHire(string bookingReference, string emailAddress)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("bookingReference", bookingReference),
                new SqlParameter("emailAddress", emailAddress)
            };

            var dataTable = DatabaseCommon.PerformAction("GetHireForBookingReferenceAndEmailAddress", parameters);
            return DatabaseCommon.ConvertRow(dataTable, Hire.FromDataRow);
        }


        public static Hire CreateHire(int userId, string bookingReference, DateTime hireEndDate, int startMileage)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("userId", userId),
                new SqlParameter("bookingReference", bookingReference),
                new SqlParameter("hireEndDate", hireEndDate == DateTime.MinValue ? DBNull.Value : (object)hireEndDate),
                new SqlParameter("startMileage", startMileage)
            };

            var dt = DatabaseCommon.PerformAction("CreateNewHire", parameters);
            return DatabaseCommon.ConvertRow(dt, Hire.FromDataRow);
        }
    }
}
