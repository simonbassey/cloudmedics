using System;
using CloudMedics.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using CloudMedics.Data;
using CloudMedics.Data.Repositories;
using System.Linq;
namespace CouldMedics.Services
{
    public interface IPatientUserService
    {
        Task<Patient> CreatePatient(Patient patient);
        Task<List<Patient>> GetPatients();
    }
    public class PatientUserService : IPatientUserService
    {
        private readonly IPatientUserRepository _patientsRepository;
        public PatientUserService(IPatientUserRepository patientUserRepository)
        {
            _patientsRepository = patientUserRepository;
        }


        public Task<Patient> CreatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetPatients()
        {
            try
            {
                var patientsList = await _patientsRepository.GetPatientsAsync();
                return patientsList.ToList();
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
