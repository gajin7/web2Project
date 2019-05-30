using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Station : IStation
    {
        [Key]
        int StationNum { get; set; }
        //koordinate??
        string Addr { get; set; }
        string Name { get; set; }
        //linije?

    }
}