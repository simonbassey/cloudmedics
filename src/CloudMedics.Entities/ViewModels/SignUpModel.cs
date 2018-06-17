using System;
using System.ComponentModel.DataAnnotations;

namespace CloudMedics.Domain.ViewModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "password is Required")]
        [StringLength(15, ErrorMessage = "Password cannot exceed 15 characters length"),
         MinLength(6, ErrorMessage = "Password must be atleast 6 characters long")]
        public string Password { get; set; }
    }

}
