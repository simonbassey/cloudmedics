using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudMedics.Domain.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CloudMedics.Domain.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [ForeignKey("UserAccount")]
        [Required]
        public string UserId { get; set; }
        public string BloodGroup { get; set; }
        public PatientType PatientType { get; set; }
        public string Occupation { get; set; }

        public virtual ApplicationUser UserAccount { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
