﻿using System;
using System.ComponentModel.DataAnnotations;
using CloudMedics.Domain.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace CloudMedics.Domain.Models
{
    public class ApplicationUser: IdentityUser {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string username):base(username){
        }

        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string FirstName  {get;set;}
        [Required]
        public string LastName { get; set; }
        public AccountType AccountType { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public char Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}