using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GeoCar.Database
{
    internal static class DatabaseCommon
    {
        internal static DataTable PerformAction(string spName, List<SqlParameter> parameters, SqlTransaction tx = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                var command = new SqlCommand
                {
                    Connection = con,
                    CommandText = spName,
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 30
                };

                if (tx != null)
                {
                    command.Transaction = tx;
                }

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                var result = new DataTable();
                using (var adaptor = new SqlDataAdapter(command))
                {
                    adaptor.Fill(result);
                }

                return result;
            }
        }

        internal static T ConvertRow<T>(DataTable dataTable, Func<DataRow, T> ifRowExists)
        {
            return (dataTable.Rows.Count > 0) ? ifRowExists(dataTable.Rows[0]) : default(T);
        }

        internal static IList<T> ConvertTable<T>(DataTable dataTable, Func<DataRow, T> forRow)
        {
            if (dataTable == null)
                return null;

            return dataTable.AsEnumerable().Select(forRow).ToList();
        }
    }
}
