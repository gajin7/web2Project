using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Station
    {
        [Key]
        public int StationNum { get; set; }
        public Location Location{ get; set; }
        public string Name { get; set; }
        public List<Line> Lines { get; set; }

    }
}