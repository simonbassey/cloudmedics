using System;
using System.Threading.Tasks;
using CloudMedics.Domain.Enumerations;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CloudMedics.Data
{
    public class AppDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AppDbInitializer> _logger;
        public AppDbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
                                ILogger<AppDbInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        public async Task Seed()
        {
            try
            {

                var superUserAccount = new ApplicationUser
                {
                    FirstName = "Super",
                    LastName = "User",
                    Email = "simon.dev.bassey@gmail.com",
                    PhoneNumber = "09077423735",
                    AccountType = AccountType.System,
                    AccountStatus = AccountStatus.Active,
                    UserName = "simon.dev.bassey@gmail.com",
                    CreatedBy = "SYSTEM",
                    EmailConfirmed = true
                };

                await InitApplicationRoles();
                var superUserAccountExist = await _userManager.FindByEmailAsync(superUserAccount.Email) != null;
                if (superUserAccountExist)
                    return;
                await CreateSuperUserAccount(superUserAccount, "develop002");

            }
            catch (Exception exception)
            {
                _logger.LogError("Error occured while trying to seed data during Database Initialization -> {0}", exception);
            }
        }

        private async Task InitApplicationRoles()
        {
            try
            {
                foreach (var role in Enum.GetValues(typeof(RoleNames)))
                {
                    if (!(await _roleManager.RoleExistsAsync(role.ToString())))
                    {
                        await _roleManager.CreateAsync(new IdentityRole() { Name = role.ToString() });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error occured while initializing application roles -> {0}", exception);
            }
        }

        private async Task CreateSuperUserAccount(ApplicationUser applicationUser, string password = "")
        {
            var userCreateResult = await _userManager.CreateAsync(applicationUser, password);
            if (userCreateResult.Succeeded)
            {
                var superUserRoleName = Enum.GetName(typeof(RoleNames), RoleNames.SuperAdministrator);
                var superUser = await _userManager.FindByEmailAsync(applicationUser.Email);
                if (!(await _userManager.IsInRoleAsync(superUser, superUserRoleName)))
                {
                    await _userManager.AddToRoleAsync(superUser, superUserRoleName);
                }
            }
        }
    }
}
