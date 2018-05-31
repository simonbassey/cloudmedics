using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudMedics.Domain.Enumerations;

namespace CloudMedics.Domain.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string BloodGroup { get; set; }
        public PatientType PatientType { get; set; }
        public string Occupation { get; set; }

        public virtual ApplicationUser UserAccount { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
