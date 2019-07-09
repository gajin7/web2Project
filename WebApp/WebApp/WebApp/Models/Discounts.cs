using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Discounts
    {
        public int Id { get; set; }
        public UserType Type { get; set; }
        public double Discount { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public double Version { get; set; }

    }
}