using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BuddyCloudCoreApi2.JwtToken.Security
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
