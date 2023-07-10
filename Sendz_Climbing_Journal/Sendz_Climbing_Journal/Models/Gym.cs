using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace Sendz_Climbing_Journal.Models
{
    public class Gym
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; } //Foreign Key with Log Table
        public string GymName { get; set; }
        public string Type { get; set; }
        public string Grade { get; set; }
        public bool Sent { get; set; }
        public string SendType { get; set; }
        public DateTime SendDate { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

    }
}
