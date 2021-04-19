﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tic_tac_toe.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastSeenOnline { get; set; }
        public string Status { get; set; }
    }
}
