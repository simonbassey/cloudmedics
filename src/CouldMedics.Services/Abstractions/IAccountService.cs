﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMedics.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CouldMedics.Services.Abstractions
{
    public interface IAccountService
    {
        Task<Tuple<IdentityResult, ApplicationUser>> CreateUserAsync(ApplicationUser user, string password = "");
        Task<ApplicationUser> GetUserAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<IEnumerable<ApplicationUser>> FilterUsersAsync(Func<ApplicationUser, bool> filterFn);
        Task<bool> UserExist(string userIdentifier);
        Task<Tuple<IdentityResult, object>> SignInUserAsync(string userName, string password);
    }
}
