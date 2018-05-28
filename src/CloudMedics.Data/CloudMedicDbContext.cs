﻿using System;
using CloudMedics.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudMedics.Data
{
    public class CloudMedicDbContext:DbContext
    {
        public CloudMedicDbContext(){}
        public CloudMedicDbContext(DbContextOptions options) : base(options)
        {
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Appointment>().HasKey(e => new { e.PatientId, e.DoctorId });
            base.OnModelCreating(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}


        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
	}
}