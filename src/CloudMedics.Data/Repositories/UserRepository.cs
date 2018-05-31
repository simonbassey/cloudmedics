using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudMedics.Domain.Models;

namespace CloudMedics.Data.Repositories
{
    public interface IUserRepository{
        
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<int> CreateUsersAsync(IList<ApplicationUser> users);
        Task<ApplicationUser> GetUserAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser> UpdateUserAccountAsync(string userId, ApplicationUser updatedUser);
        Task<bool> DeleteUserAccountAsync(string userId);
        Task<IEnumerable<ApplicationUser>> FilterUsersAsync(Func<ApplicationUser, bool> filterFn);
        Task<bool> UserAccountExistAsync(string userId);

    }

    public class UserRepository:BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository()
        {
        }

        public async Task<bool> DeleteUserAccountAsync(string userId){
            return await Delete(userId);
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            return await base.Add(user);
        }

        public async Task<int> CreateUsersAsync(IList<ApplicationUser> users)
        {
            return await Add(users);
        }

        public async Task<IEnumerable<ApplicationUser>> FilterUsersAsync(Func<ApplicationUser, bool> filterFn)
        {
            return await Filter(filterFn);
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await Get(userId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await GetAll();
        }

        public async Task<ApplicationUser> UpdateUserAccountAsync(string userId, ApplicationUser updatedUser)
        {
            var userAccount = await GetUserAsync(userId);
            if (userAccount == null)
                return null;
            return await Update(updatedUser);
        }

        public async Task<bool> UserAccountExistAsync(string userId)
        {
            return (await FilterUsersAsync(u => u.UserId.ToString().Equals(userId, StringComparison.OrdinalIgnoreCase) ||
                                    u.PhoneNumber.Equals(userId, StringComparison.OrdinalIgnoreCase) ||
                                           u.EmailAddress.Equals(userId, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault() != null;
        }
    }
}
