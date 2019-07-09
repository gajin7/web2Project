using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApp.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Number")]
        public string Number { get; set; }

        [Required]
        [Display(Name = "TypeOfPerson")]
        public string TypeOfPerson { get; set; }

    }

    public class EditProfileBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Number")]
        public string Number { get; set; }

      
        [Display(Name = "TypeOfPerson")]
        public string TypeOfPerson { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string Date { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddStationModel
    {
        [Required]
        [Display(Name = "name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "longitude")]
        public string longitude { get; set; }

        [Required]
        [Display(Name = "latitude")]
        public string latitude { get; set; }
    }

    public class ChangeStationModel
    {

        [Required]
        [Display(Name = "id")]
        public string id { get; set; }

        [Required]
        [Display(Name = "name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "longitude")]
        public string longitude { get; set; }

        [Required]
        [Display(Name = "latitude")]
        public string latitude { get; set; }

        [Required]
        [Display(Name = "version")]
       public string version { get; set; }
    }
}
