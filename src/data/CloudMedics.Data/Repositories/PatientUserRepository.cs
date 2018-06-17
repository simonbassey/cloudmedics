using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudMedics.Domain.Models;
namespace CloudMedics.Data.Repositories
{

    public interface IPatientUserRepository
    {
        Task<Patient> AddPatientAsync(Patient user);
        Task<int> AddPatientsAsync(IList<ApplicationUser> users);
        Task<Patient> GetPatientAsync(string userId);
        Task<Patient> GetPatientAsync(int patientId);
        Task<IEnumerable<Patient>> GetPatientsAsync();
        Task<Patient> UpdatePatientAsync(int patient, Patient updatedUser);
        Task<bool> DeleteAsync(int patient);
        Task<IEnumerable<Patient>> FilterPatientsAsync(Func<Patient, bool> filterFn);
        Task<bool> CheckIfPatientExistAsync(string userId);
        Task<bool> CheckIfPatientExistAsync(int patientId);
    }

    public class PatientUserRepository : BaseRepository<Patient>, IPatientUserRepository
    {
        public async Task<Patient> AddPatientAsync(Patient patientUser)
        {
            var patientInformationExist = await CheckIfPatientExistAsync(patientUser.UserId);
            if (patientInformationExist)
                return null;
            return await Add(patientUser);
        }

        public Task<int> AddPatientsAsync(IList<ApplicationUser> users)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckIfPatientExistAsync(string userId)
        {
            return (await GetPatientAsync(userId)) != null;
        }

        public async Task<bool> CheckIfPatientExistAsync(int patientId)
        {
            return await Get(patientId) != null;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            return await Delete(userId);
        }

        public Task<bool> DeleteAsync(int patient)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Patient>> FilterPatientsAsync(Func<Patient, bool> filterFn)
        {
            return await Filter(filterFn);
        }

        public async Task<Patient> GetPatientAsync(string userId)
        {
            return (await FilterPatientsAsync(p => p.UserId.ToLower().Equals(userId.ToLower()))).FirstOrDefault();
        }

        public async Task<Patient> GetPatientAsync(int patientId)
        {
            return await Get(patientId);
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            return await GetAll();
        }

        public async Task<Patient> UpdatePatientAsync(string userId, Patient updatedUser)
        {
            var patientExist = await CheckIfPatientExistAsync(userId);
            if (!patientExist)
                return null;
            return await Update(updatedUser);
        }

        public Task<Patient> UpdatePatientAsync(int patient, Patient updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
