using System;
using System.ComponentModel.DataAnnotations;
using CloudMedics.Entities.Enumerations;

namespace CloudMedics.Entities.Models
{
    public class Appointment
    {
        public Appointment()
        {
            Created = DateTime.Now;
        }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string AilmentDescription { get; set; }
        public string Symptoms { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
