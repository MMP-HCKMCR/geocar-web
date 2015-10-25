using System.Runtime.Serialization;

namespace GeoCar.WcfService.Responses
{
    public class AchievementResponse
    {
        [DataMember]
        public string AchievementName { set; get; }
        [DataMember]
        public string AchievementDescription { set; get; }
    }
}