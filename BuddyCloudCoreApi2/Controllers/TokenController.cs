using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.JwtToken.Security;
using BuddyCloudCoreApi2.Services.Interfaces;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace BuddyCloudCoreApi2.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    [Route("api/v1/Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private IAccountService _acctSvc;

        public TokenController(
            IConfiguration config,
            IAccountService acctSvc)
        {
            _config = config;
            _acctSvc = acctSvc;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody]RegistrationLoginModel model)
        {
            if (!ModelState.IsValid)
                return Unauthorized();

            var userVerified = await _acctSvc.VerifyUserAsync(model);

            if (userVerified != null)
            {
                var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(_config.GetSection("JwtSettings:SecurityKey").Value))
                                .AddSubject(model.Email)
                                .AddIssuer(_config.GetSection("AppConfiguration:Issuer").Value)
                                .AddAudience(_config.GetSection("AppConfiguration:Issuer").Value)
                                //.AddClaim("SellerId", userVerified.Id.ToString())
                                .AddExpiry(10)
                                .Build();

                TokenModel tokenModel = new TokenModel
                {
                    AccessToken = token.Value,
                    SellerId = userVerified.Id,
                    Email = model.Email
                };

                return Ok(tokenModel);
            }

            return BadRequest();
        }
    }
}