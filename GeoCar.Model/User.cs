﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCar.Model
{
    public class User : ModelBase
    {
        public int UserId { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string Surname { set; get; }
        public int Score { get; set; }
    }
}
