﻿using BuddyCloudCoreApi2.Core.Identity;
using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.Services.Interfaces;
using BuddyCloudCoreApi2.DAL.Repository;
using BuddyCloudCoreApi2.Helper;

using System.Threading.Tasks;
using System;

namespace BuddyCloudCoreApi2.Services.Service
{
    public class AccountService : IAccountService
    {
        private IEFRepository<ApplicationUser> _repo;

        public AccountService(IEFRepository<ApplicationUser> repo)
        {
            _repo = repo;
        }

        public async Task<bool> RegisterUserAsync(ApplicationUser model)
        {
            var salt = PasswordHashHelper.GenerateSalt();
            var passwordHash = PasswordHashHelper.ComputeHash(model.Password, salt);

            model.Salt = Convert.ToBase64String(salt);
            model.Password = Convert.ToBase64String(passwordHash);

            var isRegistrationSuccessful = await _repo.AddAsync(model);

            return isRegistrationSuccessful ? true : false;
        }

        public async Task<ApplicationUser> VerifyUserAsync(RegistrationLoginModel model)
        {
            var user = new ApplicationUser { Email = model.Email };

            var userInfo = await _repo.GetUserInfoAsync(u => u.Email == model.Email);
            var salt = Convert.FromBase64String(userInfo.Salt);
            var hashPassword = Convert.FromBase64String(userInfo.Password);

            var isVerified = PasswordHashHelper.VerifyPassword(model.Password, salt, hashPassword);

            return isVerified ? userInfo : null;
        }
    }
}