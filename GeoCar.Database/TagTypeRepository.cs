using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GeoCar.Model;

namespace GeoCar.Database
{
    public class TagTypeRepository
    {
        public static TagType RetrieveTagType(int tagTypeId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "TagTypeId",
                    Value = tagTypeId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetTagTypeForTageTypeId", parameters);

            return dataTable != null && dataTable.Rows.Count > 0 ? PopulateTagType(dataTable.Rows[0]) : null;
        }

        private static TagType PopulateTagType(DataRow tagType)
        {
            return new TagType
            {
                TagTypeId = tagType.Field<int>("TagTypeId"),
                TypeName = tagType.Field<string>("TypeName"),
                Points = tagType.Field<int>("Points"),
                LockoutTimePeriod = tagType.Field<int>("LockoutTimePeriod")
            };
        }
    }
}
