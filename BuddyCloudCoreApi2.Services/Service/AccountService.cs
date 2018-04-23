using BuddyCloudCoreApi2.Core.Identity;
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
            model.Id = Guid.NewGuid();
            model.Password = PasswordHashHelper.GetPasswordHash(model.Password);

            var isRegistrationSuccessful = await _repo.AddAsync(model);

            return isRegistrationSuccessful;
        }

        public async Task<ApplicationUser> VerifyUserAsync(RegistrationLoginModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email
            };

            var userInfo = await _repo.GetUserInfoAsync(user);

            var isVerified = PasswordHashHelper.VerifyPasswordHash(userInfo, model);

            if (isVerified)
                return userInfo;
            else
                return null;
        }
    }
}