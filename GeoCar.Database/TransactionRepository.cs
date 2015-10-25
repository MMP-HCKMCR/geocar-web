using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GeoCar.Model;

namespace GeoCar.Database
{
    public class TransactionRepository
    {
        public static Transaction CreateTransactionForUserAndTag(int userId, int points, int tagId, TransactionType transactionTypeId)
        {
            var transaction = CreateTransaction(userId, points, transactionTypeId);

            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "tagId",
                    Value = tagId
                },
                new SqlParameter
                {
                    ParameterName = "transactionId",
                    Value = transaction.TransactionId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("AddTransactionTag", parameters);

            return dataTable != null && dataTable.Rows.Count > 0 ? PopulateTransaction(dataTable.Rows[0]) : null;
        }

        public static Transaction CreateTransaction(int userId, int points, TransactionType transactionTypeId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId
                },
                new SqlParameter
                {
                    ParameterName = "points",
                    Value = points
                },
                new SqlParameter
                {
                    ParameterName = "transactionTypeId",
                    Value = transactionTypeId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("AddTransaction", parameters);

            return DatabaseCommon.ConvertRow(dataTable, PopulateTransaction);
        }

        public static List<Transaction> RetrieveXTransactionsForUser(int count, int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "count",
                    Value = count
                },
                new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetXTransactionsForUser", parameters);

            return dataTable != null && dataTable.Rows.Count > 0 ? PopulateTransactionList(dataTable) : new List<Transaction>();
        }

        private static List<Transaction> PopulateTransactionList(DataTable transactions)
        {
            return (from DataRow transaction in transactions.Rows select PopulateTransaction(transaction)).ToList();
        }

        private static Transaction PopulateTransaction(DataRow transaction)
        {
            return new Transaction
            {
                TransactionId = transaction.Field<int>("TransactionId"),
                UserId = transaction.Field<int>("UserId"),
                Points = transaction.Field<int>("Points"),
                TimeCaptured = transaction.Field<DateTime>("TimeCaptured"),
                TransactionTypeId = transaction.Field<int>("TransactionTypeId")
            };
        }

        public static int GetUsersUsablePoints(int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetUsersUsablePoints", parameters);

            var usablePoints = 0;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                usablePoints = dataTable.Rows[0].Field<int>("Points");
            }

            return usablePoints;
        }

        public static int GetUsersPoints(int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetUsersPoints", parameters);

            var usablePoints = 0;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                usablePoints = dataTable.Rows[0].Field<int>("Points");
            }

            return usablePoints;
        }
    }
}
