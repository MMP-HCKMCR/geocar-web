using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoCar.Model;

namespace GeoCar.Database
{
    public class SessionRepository : DatabaseBase
    {
        public Session CreateSession(string email)
        {
            var userRepository = new UserRepository();
            var user = userRepository.RetrieveUser(email);

            if (user == null)
            {
                return new Session
                {
                    ErrorNumber = 1001
                };
            }

            var parameters = new List<SqlParameter>
                {
                    new SqlParameter
                    {
                        ParameterName = "UserId",
                        Value = user.UserId
                    }
                };

            var dataTable = PerformAction("AddSession", parameters);

            return dataTable != null ? PopulateSession(dataTable.Rows[0]) : InvalidSession();
        }

        private Session PopulateSession(DataRow session)
        {
            return new Session
            {
                SessionId = session.Field<string>("SessionId"),
                UserId = session.Field<int>("UserId")
            };
        }

        public Session InvalidSession()
        {
            return new Session
            {
                ErrorNumber = 1001
            };
        }
    }
}
