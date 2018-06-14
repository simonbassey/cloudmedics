using System;
using CloudMedics.API.Controllers;
using CloudMedics.Data.Mock.Repositories;
using CouldMedics.Services.Abstractions;
using Xunit;
using CloudMedics.API.Test.Helpers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CloudMedics.API.Test
{
    public class UserAccountControllerTest
    {
        [Fact]
        public async Task GetUserAccounts_Should_Return_ApplicationUserList()
        {
            //arrange
            var userManager = RepositoryMocks.GetUserManager().Object;
            var roleManager = RepositoryMocks.GetRoleManager().Object;
            var passwordHasher = RepositoryMocks.GetIPasswordHasher().Object;
            var logger = RepositoryMocks.GetLogger<UserService>().Object;
            IConfiguration configuration = TestHelper.GetApplicationConfiguration() as IConfiguration;
            IUserService userService = new UserService(userManager, roleManager, passwordHasher, configuration, logger);

            UserAccountController accountController = new UserAccountController(userService);

            //act
            var userAccounts = await accountController.GetUserAccounts();

            //assert
            Assert.NotNull(userAccounts);

        }



    }
}
