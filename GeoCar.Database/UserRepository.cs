using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GeoCar.Model;

namespace GeoCar.Database
{
    public class UserRepository : DatabaseBase
    {
        public Session LogInUser(string emailAddress, string userPassword)
        {
            var sessionRepository = new SessionRepository();

            var user = RetrieveUser(emailAddress);

            return user != null && user.Password == userPassword ? sessionRepository.CreateSession(emailAddress) : sessionRepository.InvalidSession();
        }

        public User RetrieveUser(string email)
        {
            var parameters = new List<SqlParameter>
            { new SqlParameter
                {
                    ParameterName = "email",
                    Value = email
                }
            };

            var dataTable = PerformAction("GetUserForEmail", parameters);

            return dataTable != null && dataTable.Rows.Count > 0 ? PopulateUser(dataTable.Rows[0]) : null;
        }

        private List<User> PopulateUser(DataTable userTable)
        {
            return (from DataRow dataRow in userTable.Rows select PopulateUser(dataRow)).ToList();
        }

        private User PopulateUser(DataRow user)
        {
            return new User
            {
                UserId = user.Field<int>("UserId"),
                Email = user.Field<string>("Email"),
                Password = user.Field<string>("Password"),
                FirstName = user.Field<string>("FirstName"),
                Surname = user.Field<string>("Surname"),
                Score = user.Field<int>("Score")
            };
        }
    }
}
