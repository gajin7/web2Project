using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Picture
    {

        public int Id { get; set; }

        public byte[] ImageSource { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
    }
}