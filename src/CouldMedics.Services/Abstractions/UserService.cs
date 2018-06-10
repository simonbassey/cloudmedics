using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloudMedics.Domain.Enumerations;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CouldMedics.Services.Abstractions
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        public UserService(
                           UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
            IConfiguration appConfigSettings,
                           ILogger<UserService> logger
                          )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _configuration = appConfigSettings;
            _logger = logger;
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException($"{nameof(user)}", $"Invalid user model. Cannot ceate user with null details");

                var userAccountExist = await this.UserExist(user.Email);
                if (userAccountExist)
                    throw new InvalidOperationException($"User account with email address {user.Email} already exist");
                var userCreateResult = await AddUserAccountAsync(user);
                return userCreateResult.Item2;

            }
            catch (Exception exception)
            {
                _logger.LogError("Error occured while creating user account - Location user service - Error -> {0}", exception);
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> FilterUsersAsync(Func<ApplicationUser, bool> filterFn)
        {
            return await Task.FromResult(_userManager.Users.Where(filterFn).ToList());
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        private async Task<object> GenerateJWTSignInToken(ApplicationUser user)
        {
            try
            {

                var userClaims = await BuildUserClaims(user);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var securityToken = BuildAuthToken(credentials, userClaims);

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    expiresIn = securityToken.ValidTo
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> UserExist(string userIdentifier)
        {
            var userExistQueryResult = _userManager.Users.FirstOrDefault(user => user.Id.Equals(userIdentifier, StringComparison.OrdinalIgnoreCase) ||
                                                     user.Email.Equals(userIdentifier, StringComparison.OrdinalIgnoreCase) ||
                                                     user.PhoneNumber.Equals(userIdentifier, StringComparison.OrdinalIgnoreCase)) != null;
            return await Task.FromResult(userExistQueryResult);

        }


        public async Task<Tuple<IdentityResult, object>> SignInUserAsync(string userName, string password)
        {
            try
            {

                var user = (await FilterUsersAsync(u => u.Email.Equals(userName, StringComparison.OrdinalIgnoreCase) ||
                                                   u.PhoneNumber.Equals(userName, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
                if (user == null)
                    return Tuple.Create<IdentityResult, object>(IdentityResult.Failed(new IdentityError { Description = "username is invalid" }), null);
                var accountErrors = ValidateAccountStatus(user);
                if (accountErrors.Count > 0)
                    return Tuple.Create<IdentityResult, object>(IdentityResult.Failed(accountErrors[0]), null);
                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success)
                {
                    var jwtToken = await GenerateJWTSignInToken(user);
                    return Tuple.Create(IdentityResult.Success, jwtToken);
                }
                return Tuple.Create<IdentityResult, object>(IdentityResult.Failed(new IdentityError { Description = "invalid username or password" }), null);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error occured while validating signing credentials  for user {0} -> {1}", userName, exception);
                throw;
            }
        }



        #region privates
        private async Task<Tuple<IdentityResult, ApplicationUser>> AddUserAccountAsync(ApplicationUser user, string password = "")
        {
            try
            {
                var userCreateResult = await _userManager.CreateAsync(user, password);
                if (!userCreateResult.Succeeded)
                    return Tuple.Create<IdentityResult, ApplicationUser>(userCreateResult, null);
                var createdUser = await _userManager.FindByEmailAsync(user.Email);
                await AddUserToRole(createdUser);
                return Tuple.Create(userCreateResult, createdUser);

                //user created. next we send confirmation email
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task AddUserToRole(ApplicationUser user)
        {
            var accountRole = MapRoleFromAccountType(user.AccountType);
            if (string.IsNullOrEmpty(accountRole))
                return;
            await _userManager.AddToRoleAsync(user, accountRole);
        }


        private string MapRoleFromAccountType(AccountType accountType)
        {
            var role = accountType == AccountType.Administrator ? Enum.GetName(typeof(RoleNames), RoleNames.Administrator) :
                           accountType == AccountType.Doctor ? Enum.GetName(typeof(RoleNames), RoleNames.Doctor) :
                                                 accountType == AccountType.Staff ? Enum.GetName(typeof(RoleNames), RoleNames.Staff) :
                                                 accountType == AccountType.Patient ? Enum.GetName(typeof(RoleNames), RoleNames.User) :
                                                 accountType == AccountType.System ? Enum.GetName(typeof(RoleNames), RoleNames.SuperAdministrator) : string.Empty;
            return role;
        }

        private IList<IdentityError> ValidateAccountStatus(ApplicationUser user)
        {
            List<IdentityError> authenticationErrors = new List<IdentityError>();
            if (!user.EmailConfirmed)
                authenticationErrors.Add(new IdentityError { Description = "You have not confirmed your email address" });
            if (user.AccountStatus != AccountStatus.Active)
                authenticationErrors.Add(new IdentityError { Description = "Your account is suspended. Contact admin to reolve issues " });
            return authenticationErrors;
        }



        private async Task<IList<Claim>> BuildUserClaims(ApplicationUser user)
        {
            var assignedRoles = (await _userManager.GetRolesAsync(user));
            var rolesAsClaims = new Claim("roles", string.Join(",", assignedRoles));

            var roleClaims = new List<Claim>();
            foreach (var role in assignedRoles)
            {
                roleClaims.AddRange((await _roleManager.GetClaimsAsync(new IdentityRole() { Name = role })));
            }
            roleClaims.Add(rolesAsClaims);
            roleClaims.Union(new Claim[]{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sid, user.UserId.ToString())
                });

            return roleClaims;

        }

        private JwtSecurityToken BuildAuthToken(SigningCredentials credentials, IList<Claim> userClaims, int tokenExpiryInMinutes = 15)
        {
            return
                new JwtSecurityToken(
                    issuer: _configuration["Token:Issuer"],
                    audience: _configuration["Token:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(tokenExpiryInMinutes),
                    signingCredentials: credentials
                );

        }
        #endregion


    }
}
