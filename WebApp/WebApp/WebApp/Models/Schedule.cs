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
        public string LineName { get; set;}
        public List<KeyValuePair<DateTime,DateTime>> Depatures_Arrivals { get; set; } 


    }
}