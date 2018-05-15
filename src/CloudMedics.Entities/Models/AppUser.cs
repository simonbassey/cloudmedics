using System;
using System.ComponentModel.DataAnnotations;
using CloudMedics.Entities.Enumerations;

namespace CloudMedics.Entities
{
    public class AppUser {

        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string FirstName  {get;set;}
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
