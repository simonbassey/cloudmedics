using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudMedics.Domain.Enumerations;
using CloudMedics.Domain.Models;
using CouldMedics.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CloudMedics.Domain.ViewModels;
using AutoMapper;

namespace CloudMedics.API.Controllers
{
    [Route("api/users/account")]
    [Authorize]
    public class UserAccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper _mapper;
        public UserAccountController(IUserService userService_, IMapper mapper)
        {
            userService = userService_;
            _mapper = mapper;
        }


        /// <summary>
        /// Gets user accounts.
        /// </summary>
        /// <returns>The user accounts.</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetUserAccounts()
        {
            try
            {
                var userAccounts = await userService.GetUsersAsync();
                return Ok(userAccounts);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        ///  Creates a new user account.
        /// </summary>
        /// <returns>The account.</returns>
        /// <param name="user">User.</param>
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody] SignUpModel accountVm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var accountCreateResult = await userService.CreateUserAsync(_mapper.Map<ApplicationUser>(accountVm));
                if (!accountCreateResult.Item1.Succeeded)
                    return BadRequest(accountCreateResult.Item1.Errors);
                return accountCreateResult.Item2 == null ? StatusCode((int)HttpStatusCode.Conflict, "Failed to create user account")
                                              : Ok(accountCreateResult.Item2);

            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        #region privates

        private void UpdateAnonymousUserRegistrationData(ref ApplicationUser newUserData)
        {
            newUserData.AccountType = AccountType.Patient;
            newUserData.Created = DateTime.Now;
            newUserData.LastUpdate = DateTime.Now;
            newUserData.CreatedBy = "System";
        }
        #endregion
    }
}
