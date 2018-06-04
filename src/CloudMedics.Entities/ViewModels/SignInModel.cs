using System;
using System.ComponentModel.DataAnnotations;

namespace CloudMedics.Domain.ViewModels
{
    public class SignInModel
    {
        [Required(ErrorMessage = "")]
        [EmailAddress(ErrorMessage = "Email Address is invalid")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }

    }
}
