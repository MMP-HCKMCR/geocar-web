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
    public class AchievementRepository
    {
        public static List<Achievement> RetrieveAvailableAchievements(int userId, int tagId)
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
                    ParameterName = "tagId",
                    Value = tagId
                }
            };

            var dataTable = DatabaseCommon.PerformAction("GetNewAchievements", parameters);

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return null;
            }

            var result = PopulateAchievements(dataTable);

            foreach (var achievement in result)
            {
                CreateUserAchievement(achievement.AchievementId, userId);
            }

            return result;
        }

         public static void CreateUserAchievement(int achievementId, int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "achievementId",
                    Value = achievementId
                },
                new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId
                }
            };

            DatabaseCommon.PerformAction("AddUserAchievement", parameters);
        }

        private static List<Achievement> PopulateAchievements(DataTable achievements)
        {
            return (from DataRow achievement in achievements.Rows select PopulateAchievement(achievement)).ToList();
        }

        private static Achievement PopulateAchievement(DataRow achievement)
        {
            var result = new Achievement();

            result.AchievementId = achievement.Field<int>("AchievementId");
            result.AchievementName = achievement.Field<string>("AchievementName");
            result.AchievementDescription = achievement.Field<string>("AchievementDescription");
            result.AchievementTypeName = achievement.Field<string>("AchievementTypeName");
            result.RewardPoints = achievement.Field<int>("RewardPoints");
            result.Active = achievement.Field<bool>("Active");
            result.AchievementPoints = achievement.Field<int>("AchievementPoints");
            result.AchievementTotalTags = achievement.Field<int>("AchievementTotalTags");
            result.AchievementTypeId = achievement.Field<int>("AchievementTypeId");
            result.TagUUID = achievement.Field<Guid?>("UUID");
            result.TagMajorNumber = achievement.Field<int?>("MajorNumber");
            result.TagMinorNumber = achievement.Field<int?>("MinorNumber");

            return result;
        }
    }
}
