using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Station
    {
        [Key]
        public int StationNum { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public virtual List<Line> Lines { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public double Version { get; set; }

    }
}