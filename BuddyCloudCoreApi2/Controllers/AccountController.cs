using BuddyCloudCoreApi21.Core.Identity;
using BuddyCloudCoreApi21.Core.Models;
using BuddyCloudCoreApi21.Core.Response;
using BuddyCloudCoreApi21.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BuddyCloudCoreApi2.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _acctSvc;

        public AccountController(IAccountService acctSvc)
        {
            _acctSvc = acctSvc;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Create([FromBody] RegistrationLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(modelError => modelError.ErrorMessage).ToList());
            }

            var user = new ApplicationUser { Email = model.Email, Password = model.Password };
            var result = await _acctSvc.RegisterUserAsync(user);

            if (!result)
            {
                return BadRequest(new ResponseModel { Status = false, Message = "Error registering new user." });
            }

            return Ok(new ResponseModel { Status = true, Message = "Successfully registered user." });
        }
    }
}