using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Schedule 
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public Line Line { get; set;}
        public List<DateTime> Depatures { get; set; } 
        public TimeSpan Duration { get; set; }


    }
}