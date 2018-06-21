using System;

namespace BuddyCloudCoreApi21.Core.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public Guid SellerId { get; set; }
        public string Email { get; set; }
    }
}