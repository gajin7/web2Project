using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Schedule 
    {
        public int Id { get; set; }
        public Day Day { get; set; }
        
        public int LineId { get; set; }
        public Line Line { get; set;}
        public virtual List<Depature> Depatures { get; set; } 
        public TimeSpan Duration { get; set; }

        public double Version { get; set; }


    }
}