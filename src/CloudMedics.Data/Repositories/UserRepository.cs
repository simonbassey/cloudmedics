using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMedics.Entities;

namespace CloudMedics.Data.Repositories
{
    public interface IUserRepository{
        Task<AppUser> CreateUser(AppUser user);
        Task<int> CreateUsers(IList<AppUser> users);
        Task<IEnumerable<AppUser>> GetUsers();
    }

    public class UserRepository:BaseRepository<AppUser>, IUserRepository
    {
        public UserRepository()
        {
        }

        public async Task<AppUser> CreateUser(AppUser user)
        {
            return await base.Add(user);
        }

        public async Task<int> CreateUsers(IList<AppUser> users)
        {
            return await Add(users);
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await GetAll();
        }
    }
}
