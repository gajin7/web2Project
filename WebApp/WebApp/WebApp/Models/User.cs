using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class User
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }

        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; } 

        public Enums.UserType UserType { get; set; }

        public bool Approved { get; set; }

        public bool Checked { get; set; }

        public bool postedImage { get; set; }


    }
}