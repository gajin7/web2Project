using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Discounts
    {
        public UserType Type { get; set; }
        public double Discount { get; set; }

    }
}