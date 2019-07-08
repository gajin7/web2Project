using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PayPalInfo
    {
        [Key]
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public int TicketId { get; set; }
        public string PayerEmail { get; set; }
        public string PayerId { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public string Status { get; set; }

    }
}