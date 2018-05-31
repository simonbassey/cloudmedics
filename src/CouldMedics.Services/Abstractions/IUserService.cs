using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMedics.Domain.Models;

namespace CouldMedics.Services.Abstractions
{
    public interface  IUserService
    {
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<IEnumerable<ApplicationUser>> FilterUsersAsync(Func<ApplicationUser, bool> filterFn);
    }
}
