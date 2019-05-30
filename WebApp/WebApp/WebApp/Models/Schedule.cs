using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Schedule : ISchedule
    {
        ILine Line { get; set; }

    }
}