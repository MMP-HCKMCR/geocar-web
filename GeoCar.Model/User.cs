using System;

namespace GeoCar.Model
{
    public class User : ModelBase
    {
        public int UserId { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string Surname { set; get; }
        public DateTime LastScoreTime { get; set; }
    }
}
