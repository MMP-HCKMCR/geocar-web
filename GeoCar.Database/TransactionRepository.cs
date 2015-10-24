using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

            return dataTable != null && dataTable.Rows.Count > 0 ? PopulateTransaction(dataTable.Rows[0]) : null;
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
    }
}
