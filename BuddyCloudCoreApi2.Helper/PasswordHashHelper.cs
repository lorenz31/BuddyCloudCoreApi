using BuddyCloudCoreApi2.Core.Identity;
using BuddyCloudCoreApi2.Core.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BuddyCloudCoreApi2.Helper
{
    public class PasswordHashHelper
    {
        public static string GetPasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static bool VerifyPasswordHash(ApplicationUser user, RegistrationLoginModel model)
        {
            var passwordHash = GetPasswordHash(model.Password);

            if (user.Password == passwordHash)
                return true;
            else
                return false;
        }
    }
}