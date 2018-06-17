using System;
using Microsoft.AspNetCore.Mvc;
using CouldMedics.Services;
using System.Net;
using System.Threading.Tasks;
using CouldMedics.Services.Abstractions;
using System.Linq;
using CloudMedics.Domain.Models;

namespace CloudMedics.API.Controllers
{
    [Route("api/users/patients")]
    public class PatientUsersController : Controller
    {
        private readonly IPatientUserService _patientUserService;
        private readonly IUserService _accountService;
        public PatientUsersController(IPatientUserService patientService, IUserService accountService)
        {
            _patientUserService = patientService;
            _accountService = accountService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPatients()
        {
            try
            {
                var patients = await _patientUserService.GetPatients();
                foreach (Patient patient in patients)
                {
                    if (patient.UserAccount == null)
                        patient.UserAccount = await _accountService.GetUserAsync(patient.UserId);
                }
                return Ok(patients);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
