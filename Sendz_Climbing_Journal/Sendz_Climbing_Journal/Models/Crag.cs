using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace Sendz_Climbing_Journal.Models
{
    public class Crag
    {   
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; } //Foreign Key to Log Table
        public string State { get; set; }
        public string CragName { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public bool Sent { get; set; }
        public string SendType { get; set; }
        public DateTime SendDate { get; set; }
        public string Notes { get; set; }

    }
}
