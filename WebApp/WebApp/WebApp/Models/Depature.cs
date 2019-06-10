using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Depature
    {
        public int Id { get; set; }
        public string DepatureTime { get; set; }
        public Schedule Schedule { get; set; }
    }
}