using System;
using Microsoft.AspNetCore.Mvc;
using CouldMedics.Services;
using System.Net;
using System.Threading.Tasks;
using CouldMedics.Services.Abstractions;
using System.Linq;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace CloudMedics.API.Controllers
{
    [Route("api/users/patients")]
    [Authorize]
    public class PatientUsersController : Controller
    {
        private readonly IPatientUserService _patientUserService;
        private readonly IAccountService _accountService;
        public PatientUsersController(IPatientUserService patientService, IAccountService accountService)
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
