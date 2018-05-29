using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMedics.Domain.Models;

namespace CouldMedics.Services.Abstractions
{
    public interface  IUserService
    {
        Task<AppUser> CreateUserAsync(AppUser user);
        Task<AppUser> GetUserAsync(string userId);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<AppUser>> FilterUsersAsync(Func<AppUser, bool> filterFn);
    }
}
