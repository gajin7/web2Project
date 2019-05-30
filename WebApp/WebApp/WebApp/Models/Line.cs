using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Line : ILine
    {
        [Key]
        public int LineNum { get; set; }
        public IEnumerable<IStation> Stations { get; set; }
        public LineTypes LineType { get; set; }
        
    }
}