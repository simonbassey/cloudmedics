using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudMedics.Domain.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        [ForeignKey("UserAccount")]
        [Required]
        public string UserId { get; set; }
        public string ProfileSummary { get; set; }

        public ApplicationUser UserAccount { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
