using BuddyCloudCoreApi21.Core.Models;
using BuddyCloudCoreApi21.Core.Identity;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi21.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterUserAsync(ApplicationUser model);
        Task<ApplicationUser> VerifyUserAsync(RegistrationLoginModel model);
    }
}