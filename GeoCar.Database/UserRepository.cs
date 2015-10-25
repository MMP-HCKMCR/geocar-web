using System;
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

        public static User CreateUser(string emailAddress)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("emailAddress", emailAddress)
            };

            var dt = DatabaseCommon.PerformAction("CreateNewUser", parameters);
            return DatabaseCommon.ConvertRow(dt, PopulateUser);
        }

        public static User RetrieveUser(int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("userId", userId)
            };

            var dt = DatabaseCommon.PerformAction("GetUserForUserId", parameters);
            return DatabaseCommon.ConvertRow(dt, PopulateUser);
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

        public static User UpdateUser(User user)
        {
            var parameters = new List<SqlParameter>
            { new SqlParameter
                {
                    ParameterName = "UserId",
                    Value = user.UserId
                },
                new SqlParameter
                {
                    ParameterName = "Email",
                    Value = user.Email
                },
                new SqlParameter
                {
                    ParameterName = "Password",
                    Value = user.Password
                },
                new SqlParameter
                {
                    ParameterName = "FirstName",
                    Value = user.FirstName
                },
                new SqlParameter
                {
                    ParameterName = "Surname",
                    Value = user.Surname
                },
                new SqlParameter
                {
                    ParameterName = "Score",
                    Value = user.Score
                }
            };

            var dataTable = DatabaseCommon.PerformAction("UpdateUser", parameters);

            return DatabaseCommon.ConvertRow(dataTable, PopulateUser);
        }

        public static User RetrieveUser(Guid sessionId)
        {
            var parameters = new List<SqlParameter>
            { new SqlParameter
                {
                    ParameterName = "SessionId",
                    Value = sessionId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetUserForSessionId", parameters);

            return DatabaseCommon.ConvertRow(dataTable, PopulateUser);
        }

        public static IList<LeaderboardEntry> RetrieveTopLeaderboard(int rowCount, int? currentUserId = null)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("rowCount", rowCount),
                new SqlParameter("userId", currentUserId)
            };

            var dataTable = DatabaseCommon.PerformAction("GetTopLeaderboardInfo", parameters);

            return DatabaseCommon.ConvertTable(dataTable, LeaderboardEntry.FromDataRow);
        }

        public static IList<LeaderboardEntry> RetrieveLocalLeaderboard(int userId, int locality = 2)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("userId", userId),
                new SqlParameter("locality", locality)           
            };

            var dataTable = DatabaseCommon.PerformAction("GetLocalLeaderboardInfo", parameters);

            return DatabaseCommon.ConvertTable(dataTable, LeaderboardEntry.FromDataRow);
        }

        public static int RetrieveUserRanking(int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("userId", userId)
            };

            var dataTable = DatabaseCommon.PerformAction("GetUserRanking", parameters);

            return DatabaseCommon.ConvertRow(dataTable, (row) => row.Field<int>("Ranking"));
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
                Score = user.Field<int>("Score"),
                LastScoreTime = user.Field<DateTime>("LastScoreTime")
            };
        }
    }
}
