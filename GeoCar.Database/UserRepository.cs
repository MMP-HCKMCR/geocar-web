using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GeoCar.Model;

namespace GeoCar.Database
{
    public static class UserRepository
    {
        public static Session LogInUser(string emailAddress, string userPassword)
        {
            var user = RetrieveUser(emailAddress);

            return user != null && user.Password == userPassword ? SessionRepository.CreateSession(emailAddress) : null;
        }

        public static User RetrieveUser(string email)
        {
            var parameters = new List<SqlParameter>
            { new SqlParameter
                {
                    ParameterName = "email",
                    Value = email
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetUserForEmail", parameters);

            return DatabaseCommon.ConvertRow(dataTable, PopulateUser);
        }

        private static List<User> PopulateUser(DataTable userTable)
        {
            return (from DataRow dataRow in userTable.Rows select PopulateUser(dataRow)).ToList();
        }

        private static User PopulateUser(DataRow user)
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
