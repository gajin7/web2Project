using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Line
    {
        [Key]
        public string Name { get; set; }
        public List<Station> Stations { get; set; }
        public List<Schedule> Schedules { get; set; }
        public LineTypes LineType { get; set; }
        public List<Location> Locations { get; set; }
        
    }
}