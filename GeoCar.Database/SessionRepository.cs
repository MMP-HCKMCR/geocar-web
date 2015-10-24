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
    public static class SessionRepository
    {
        public static Session CreateSession(string email)
        {
            var user = UserRepository.RetrieveUser(email);

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

            var dataTable = DatabaseCommon.PerformAction("AddSession", parameters);

            return DatabaseCommon.ConvertRow(dataTable, PopulateSession, InvalidSession);
        }

        public static Session RetrieveSession(Guid sessionId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter ("sessionId", sessionId)
            };

            var dataTable = DatabaseCommon.PerformAction("GetSessionForId", parameters);

            return DatabaseCommon.ConvertRow(dataTable, PopulateSession, InvalidSession);
        }

        private static Session PopulateSession(DataRow session)
        {
            return new Session
            {
                SessionId = session.Field<string>("SessionId"),
                UserId = session.Field<int>("UserId")
            };
        }

        public static Session InvalidSession()
        {
            return new Session
            {
                ErrorNumber = 1001
            };
        }
    }
}
