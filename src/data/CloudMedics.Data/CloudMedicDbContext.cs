using System;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExpenseMgr.Data;
using CloudMedics.Data.Helpers;

namespace CloudMedics.Data
{
    public class CloudMedicDbContext : IdentityDbContext<ApplicationUser>
    {
        public CloudMedicDbContext() { }
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
            IConfiguration configuration = ConfigSettingsHelper.GetConfiguration();
            var connectionString = configuration.GetConnectionString("cloudmedicsDbConnection");
            optionsBuilder.UseMySql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
    }
}
