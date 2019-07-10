using System;
using System.Collections.Generic;
using System.Drawing;

namespace WebApp.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Date { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }

       // public bool HasRegistered { get; set; }

       // public string LoginProvider { get; set; }
    }

    public class LinesModel
    {
        public string Line { get; set; }

        public string Version { get; set; }
        
    }

    public class ScheduleModel
    {
        public string Depatures { get; set; }

        public string Message { get; set; }

        public string Version { get; set; }

    }

    public class StationsModel
    {
        public string Station { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class StationViewModel
    {
        public string name { get; set; }

        public string address { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

    public class TicketViewModel
    {
        public string id { get; set; }
        public string mail { get; set; }

        public string type { get; set; }
        public string price { get; set; }

        public string mssg { get; set; }
    }

    public class StationModel
    {
        public string name { get; set; }
        public double longitude { get; set; }

        public double latitude { get; set; }

        public string address { get; set; }

        public string lines { get; set; }
    }

    public class LineModel
    {
        public string LineNumber { get; set; }
        public List<Stations> Stations { get; set; }

        public string ColorLine { get; set; }
    }

    public class Stations
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }


    public class LineViewModel
    {
        public string name { get; set; }

        public string stations { get; set; }
       
    }

    public class ControlInfoModel
    {
        public string Email { get; set; }

        public string Type { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string Image { get; set; }
    }
}
