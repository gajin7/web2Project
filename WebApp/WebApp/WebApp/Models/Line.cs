using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Windows.Input;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Line
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Station> Stations { get; set; }
        public LineTypes LineType { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public double Version { get; set; }

    }
}