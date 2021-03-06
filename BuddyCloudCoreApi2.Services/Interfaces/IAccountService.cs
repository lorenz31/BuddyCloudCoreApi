﻿using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.Core.Identity;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi2.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterUserAsync(ApplicationUser model);
        Task<ApplicationUser> VerifyUserAsync(RegistrationLoginModel model);
    }
}