using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using GeoCar.Model;

namespace GeoCar.WcfService.Responses
{
    public class LeaderboardResponseObject
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public DateTime LastScoreTime { get; set; }

        [DataMember]
        public int Score { get; set; }

        public static LeaderboardResponseObject FromModel(LeaderboardEntry entry)
        {
            return new LeaderboardResponseObject
            {
                FirstName = entry.FirstName,
                Surname = entry.Surname,
                Score = entry.Score,
                LastScoreTime = entry.LastScoreTime
            };
        }
    }
}