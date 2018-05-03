using BuddyCloudCoreApi2.JwtToken.Security;

using System;

namespace BuddyCloudCoreApi2.Core.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public Guid SellerId { get; set; }
        public string Email { get; set; }
    }
}