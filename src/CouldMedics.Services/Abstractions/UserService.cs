using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloudMedics.Data.Repositories;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CouldMedics.Services.Abstractions
{
    public class UserService:IUserService
    {
        private readonly IUserRepository  userRepository;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IConfigurationRoot _configuration;
        public UserService(IUserRepository _userRepository, 
                           UserManager<ApplicationUser> userManager, 
                           RoleManager<ApplicationUser> roleManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
                           IConfigurationRoot appConfigSettings
                          )
        {
            userRepository = _userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _configuration = appConfigSettings;
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            try{
                if (user == null)
                    throw new  ArgumentNullException($"{nameof(user)}", $"Invalid user model. Cannot ceate user with null details");

                var userAccountExist = await userRepository.UserAccountExistAsync(user.Email);
                if (userAccountExist)
                    throw new InvalidOperationException($"User account with email address {user.Email} already exist");
                return await userRepository.CreateUserAsync(user);
                                                 
            }
            catch(Exception exception) {
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> FilterUsersAsync(Func<ApplicationUser, bool> filterFn)
        {
            return await userRepository.FilterUsersAsync(filterFn);
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await userRepository.GetUserAsync(userId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await userRepository.GetUsersAsync();
        }

        private async Task<object> GenerateJWTSignInToken(ApplicationUser user) {
            try{

                var claims = await _roleManager.GetClaimsAsync(user);
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Sid, user.UserId.ToString()));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var securityToken = new JwtSecurityToken(
                    issuer: _configuration["Token:issuer"],
                    audience: _configuration["Token:audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials
                );

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    expiresIn = securityToken.ValidTo
                };
            }
            catch(Exception) {
                throw;
            }
        }
    }
}
