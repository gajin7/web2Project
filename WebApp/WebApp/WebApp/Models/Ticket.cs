using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public TicketType Type { get; set; }
        public TimeSpan RemainingTime { get; set;}
        public bool Checked { get; set; }

        public string CheckedTime { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}