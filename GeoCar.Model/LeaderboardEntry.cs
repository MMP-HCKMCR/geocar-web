using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GeoCar.Model
{
    public class LeaderboardEntry
    {
        public int Position { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Score { get; set; }
        public DateTime LastScoreTime { get; set; }
        public bool IsCurrentUser { get; set; }

        public static LeaderboardEntry FromDataRow(DataRow dr)
        {
            return new LeaderboardEntry
            {
                Position = (int)dr.Field<long>("Position"),
                FirstName = dr.Field<string>("FirstName"),
                Surname = dr.Field<string>("Surname"),
                Score = dr.Field<int>("Score"),
                LastScoreTime = dr.Field<DateTime>("LastScoreTime"),
                IsCurrentUser = dr.Field<int>("IsCurrentUser") == 0
            };
        }
    }
}
