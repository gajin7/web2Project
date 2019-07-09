using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Prices
    {
        public int Id { get; set; }
        public TicketType ticketType { get; set; }
        public double price { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public double Version { get; set; }
    }
}