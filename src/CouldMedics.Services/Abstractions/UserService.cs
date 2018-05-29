using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMedics.Data.Repositories;
using CloudMedics.Domain.Models;

namespace CouldMedics.Services.Abstractions
{
    public class UserService:IUserService
    {
        private readonly IUserRepository  userRepository;
        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            try{
                if (user == null)
                    throw new  ArgumentNullException($"{nameof(user)}", $"Invalid user model. Cannot ceate user with null details");

                var userAccountExist = await userRepository.UserAccountExistAsync(user.EmailAddress);
                if (userAccountExist)
                    throw new InvalidOperationException($"User account with email address {user.EmailAddress} already exist");
                return await userRepository.CreateUserAsync(user);
                                                 
            }
            catch(Exception exception) {
                throw;
            }
        }

        public async Task<IEnumerable<AppUser>> FilterUsersAsync(Func<AppUser, bool> filterFn)
        {
            return await userRepository.FilterUsersAsync(filterFn);
        }

        public async Task<AppUser> GetUserAsync(string userId)
        {
            return await userRepository.GetUserAsync(userId);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await userRepository.GetUsersAsync();
        }
    }
}
