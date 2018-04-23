using BuddyCloudCoreApi2.DAL.Repository;
using BuddyCloudCoreApi2.DAL.DataContext;
using BuddyCloudCoreApi2.Helper;
using BuddyCloudCoreApi2.Core.Identity;
using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.JwtToken.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BuddyCloudCoreApi2.UnitTest
{
    [TestClass]
    public class UserAccountUnitTest
    {
        GenericRepository<ApplicationUser> _repo;
        DatabaseContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();
            options.UseSqlServer(DbConnectionStringHelper.CONNECTIONSTRING);

            _dbContext = new DatabaseContext(options.Options);

            _repo = new GenericRepository<ApplicationUser>(_dbContext);
        }

        [TestMethod]
        public async Task Account_RegisterNewUser_Test()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "seller1@gmail.com",
                Password = PasswordHashHelper.GetPasswordHash("64a64sd6a4d6sa54d6sa4d56s4d56d")
            };

            var isRegistrationSuccessful = await _repo.AddAsync(user);

            Assert.IsTrue(isRegistrationSuccessful);
        }

        [TestMethod]
        public async Task Account_UserLoginAndTokenGeneration_Test()
        {
            string email = "seller1@gmail.com";
            string pass = "64a64sd6a4d6sa54d6sa4d56s4d56d";

            var user = new ApplicationUser
            {
                Email = email
            };

            var userInfo = await _repo.GetUserInfoAsync(user);

            var loginInfo = new RegistrationLoginModel
            {
                Email = email,
                Password = pass
            };

            var isVerified = PasswordHashHelper.VerifyPasswordHash(userInfo, loginInfo);

            if (isVerified)
            {
                var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("BuddyCloudC0r34p1$3cr3t"))
                                .AddSubject(loginInfo.Email)
                                .AddIssuer("http://localhost:65099/")
                                .AddAudience("http://localhost:65099/")
                                //.AddClaim("MembershipId", "111")
                                .AddExpiry(10)
                                .Build();

                Assert.IsNotNull(token);
            }
            else
                Assert.IsTrue(false);
        }
    }
}