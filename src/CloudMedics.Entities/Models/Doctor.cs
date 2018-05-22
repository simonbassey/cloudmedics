using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudMedics.Entities.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string UserId { get; set; }
        public string ProfileSummary { get; set; }

        public AppUser UserAccount { get; set; }
        public ICollection<Appointment> Appointments {get;set;}
    }
}
