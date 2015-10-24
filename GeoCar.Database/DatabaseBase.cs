using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GeoCar.Database
{
    public class DatabaseBase
    {
        protected DataTable PerformAction(string spName, List<SqlParameter> parameters)
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
    }
}
