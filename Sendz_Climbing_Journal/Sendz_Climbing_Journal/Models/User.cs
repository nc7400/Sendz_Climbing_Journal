using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Sendz_Climbing_Journal.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
