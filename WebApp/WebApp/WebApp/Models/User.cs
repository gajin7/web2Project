using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class User
    {
       

        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; } 

        public Enums.UserType UserType { get; set; }

    }
}