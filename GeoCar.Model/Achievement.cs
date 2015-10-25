using System;

namespace GeoCar.Model
{
    public class Achievement
    {
        public int AchievementId { get; set; }
        public int AchievementPoints { get; set; }
        public int AchievementTotalTags { get; set; }
        public string AchievementName { get; set; }
        public string AchievementDescription { get; set; }
        public int AchievementTypeId { get; set; }
        public int RewardPoints { get; set; }
        public string AchievementTypeName { get; set; }
        public Guid? TagUUID { get; set; }
        public int? TagMajorNumber { get; set; }
        public int? TagMinorNumber { get; set; }
        public bool Active { get; set; }
    }
}
