using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class GetAchievementResponse : ApiResult
    {
        [DataMember]
        public AchievementResponse[] UsersAchievements { set; get; }
        [DataMember]
        public AchievementResponse[] RemainingAchievements { get; set; }
    }
}