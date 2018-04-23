using System.ComponentModel.DataAnnotations;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class RegistrationLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}