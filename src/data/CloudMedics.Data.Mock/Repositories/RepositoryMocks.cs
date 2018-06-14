using System;
using Moq;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Identity;
using CloudMedics.Data.Mock.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CloudMedics.Data.Mock.Repositories
{
    public class RepositoryMocks
    {
        private static List<ApplicationUser> mockUserList = DataReader.Read<List<ApplicationUser>>("users").Result;
        public static Mock<UserManager<ApplicationUser>> GetUserManager()
        {
            var mockUserStore = GetIUserStore();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object);
            mockUserManager.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
                           .Returns<ApplicationUser>(user => Task.FromResult(mockUserList.FirstOrDefault()));

            mockUserManager.Setup(userManager => userManager.Users)
                           .Returns(mockUserList.AsQueryable());
            return mockUserManager;
        }

        public static Mock<IUserStore<ApplicationUser>> GetIUserStore()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserStore.Setup(userStoreBase => userStoreBase.FindByNameAsync(It.IsAny<string>(), new CancellationToken()))
                         .Returns(() => Task.FromResult(mockUserList.FirstOrDefault()));

            return mockUserStore;
        }

        public static Mock<IPasswordHasher<ApplicationUser>> GetIPasswordHasher()
        {
            var mockPasswordHasherImpl = new Mock<IPasswordHasher<ApplicationUser>>();
            return mockPasswordHasherImpl;
        }

        public static Mock<ILogger<T>> GetLogger<T>()
        {
            var mockLogger = new Mock<ILogger<T>>();
            return mockLogger;
        }

        public static Mock<RoleManager<IdentityRole>> GetRoleManager()
        {
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>();

            return mockRoleManager;
        }

    }
}
