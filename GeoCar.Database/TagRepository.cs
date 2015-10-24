using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GeoCar.Model;

namespace GeoCar.Database
{
    public static class TagRepository
    {
        public static Tag RetrieveTag(Guid UUID, int majorNumber, int minorNumber)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "UUID",
                    Value = UUID
                },
                new SqlParameter
                {
                    ParameterName = "majorNumber",
                    Value = majorNumber
                },
                new SqlParameter
                {
                    ParameterName = "minorNumber",
                    Value = minorNumber
                }
            };




            var dataTable = DatabaseCommon.PerformAction("GetTagForUUIDMajorAndMinorNumber", parameters);

            return dataTable != null && dataTable.Rows.Count > 0 ? PopulateTag(dataTable.Rows[0]) : null;
        }

        private static Tag PopulateTag(DataRow tag)
        {
            return new Tag
            {
                TagId = tag.Field<int>("TagId"),
                UUID = tag.Field<Guid>("UUID").ToString(),
                MajorNumber = tag.Field<int>("MajorNumber"),
                MinorNumber = tag.Field<int>("MinorNumber"),
                Active = tag.Field<bool>("Active"),
                AdditionalPoints = tag.Field<int>("AdditionalPoints"),
                TagTypeId = tag.Field<int>("TagTypeId")
            };
        }
    }
}
