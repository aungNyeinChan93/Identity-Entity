using Identity_Entity.api_06.Models.Accounts;
using Identity_Entity.api_06.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Entity.api_06.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]CreateUserRequestModel requestModel)
        {
            var result = await _accountService.CreateUserAsync(requestModel);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel requestModel)
        {
            var isLoginSuccess = await _accountService.LoginAsync(requestModel, HttpContext);
            if (!isLoginSuccess)
            {
                return Unauthorized();
            }
            return Ok("Login Success");
        }

        [Authorize]
        [HttpPost("whoAmI")]
        public async Task<IActionResult> WhoAmI()
        {
            var ctx = HttpContext.User;
            var email = ctx.Identities.First().Claims.ToList()[1].Value;
            var user = await _accountService.GetUser(email);
            return Ok(user!);
        }



    }
}
