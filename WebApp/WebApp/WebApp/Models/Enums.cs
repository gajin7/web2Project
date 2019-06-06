using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Enums
    {
        public enum LineTypes
        {
            Urban = 0,
            Suburban = 1
        }

        public enum Day
        {
            WorkDay = 0,
            Suterday,
            Sunday
        }

        public enum TicketType
        {
            TimeTicket = 0,
            DailyTicket,
            MonthlyTicekt,
            AnnualTicket  
        }

        public enum UserType
        {
            regular = 0,
            student,
            retiree

        }
    }
}