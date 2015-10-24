using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using GeoCar.Model;

namespace GeoCar.WcfService.Responses
{
    public class LeaderboardResponse : ApiResult
    {
        [DataMember]
        public int Ranking { get; set; }

        [DataMember]
        public LeaderboardResponseEntry[] Top10 { get; set; }

        [DataMember]
        public LeaderboardResponseEntry[] Contenders { get; set; }
    }

    public class LeaderboardResponseEntry
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public DateTime LastScoreTime { get; set; }

        [DataMember]
        public int Score { get; set; }

        public static LeaderboardResponseEntry FromModel(LeaderboardEntry entry)
        {
            return new LeaderboardResponseEntry
            {
                FirstName = entry.FirstName,
                Surname = entry.Surname,
                Score = entry.Score,
                LastScoreTime = entry.LastScoreTime
            };
        }
    }
}