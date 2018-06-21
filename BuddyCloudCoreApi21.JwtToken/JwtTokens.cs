using System;
using System.IdentityModel.Tokens.Jwt;

namespace BuddyCloudCoreApi21.JwtToken
{
    public sealed class JwtTokens
    {
        private JwtSecurityToken token;

        internal JwtTokens(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime ValidTo => token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);
    }
}
