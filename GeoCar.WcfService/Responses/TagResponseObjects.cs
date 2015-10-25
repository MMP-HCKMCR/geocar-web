using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;

namespace GeoCar.WcfService.Responses
{
    public class RegisterTagResponse : ApiResult
    {
        [DataMember]
        public int PointsScored { get; set; }
        [DataMember]
        public int NewPointsTotal { get; set; }
        [DataMember]
        public int UsablePoints { get; set; }
        [DataMember]
        public string[] Achievements { get; set; }
        [DataMember]
        public LeaderboardResponseEntry[] Top10 { get; set; }
        [DataMember]
        public int Ranking { get; set; }
        [DataMember]
        public int LockoutTime { get; set; }
    }
}