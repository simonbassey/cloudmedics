using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudMedics.Domain.ViewModels;
using CouldMedics.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CloudMedics.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private ILogger<AuthController> _logger;
        public AuthController(IUserService userService, ILogger<AuthController> logger) {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("Token")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateUser([FromBody] SignInModel signInCredentials) {
            try{
                var signInResult = await _userService.SignInUserAsync(signInCredentials.EmailAddress, signInCredentials.Password);
                if(!signInResult.Item1.Succeeded) {
                    var authFailure = signInResult.Item1;
                    return BadRequest(authFailure.Errors);
                }
                _logger.LogDebug("User {0} logged in at {1}", signInCredentials.EmailAddress, DateTime.Now);
                return Ok(signInResult.Item2);
            }
            catch(Exception exception) {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);  
            }
        }
    }
}
