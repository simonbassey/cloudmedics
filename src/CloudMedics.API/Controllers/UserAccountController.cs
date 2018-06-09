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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CloudMedics.API.Controllers
{
    [Route("api/users/account")]
    [Authorize]
    public class UserAccountController : Controller
    {
        private readonly IUserService userService;
        public UserAccountController(IUserService userService_)
        {
            userService = userService_;
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
        public async Task<IActionResult> CreateAccount([FromBody] ApplicationUser user)
        {
            //Todo: Create a user model to control amount of information received by users
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                UpdateAnonymousUserRegistrationData(ref user);
                var newAccount = await userService.CreateUserAsync(user);
                return newAccount != null ? Ok(newAccount) : StatusCode((int)HttpStatusCode.Conflict, "Failed to create user account");

            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        #region privates

        private void UpdateAnonymousUserRegistrationData(ref ApplicationUser newUserData) {
            newUserData.AccountType = AccountType.Patient;
            newUserData.Created = DateTime.Now;
            newUserData.LastUpdate = DateTime.Now;
            newUserData.CreatedBy = "System";
        }
        #endregion
    }
}
