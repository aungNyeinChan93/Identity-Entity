using Identity_Entity.api_06.Models.Accounts;
using Identity_Entity.api_06.Services;
using Microsoft.AspNetCore.Authentication;
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


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel requestModel)
        {
            var auth = await HttpContext.AuthenticateAsync("cookieAuth");

            var isLoginSuccess = await _accountService.LoginAsync(requestModel, HttpContext);
            if (!isLoginSuccess)
            {
                return Unauthorized();
            }
            return Ok("Login Success");
        }

        [Authorize]
        [HttpGet("whoAmI")]
        public async Task<IActionResult> WhoAmI()
        {
            //if (!User.Identity!.IsAuthenticated)
            //{
            //    return Unauthorized();
            //}
            //return Ok("auth");
            var ctx = HttpContext.User;
            var email = ctx.Identities.First().Claims.ToList()[2].Value;
            var user = await _accountService.GetUser(email);
            return Ok(user);
        }

        [Authorize(Policy = "admin-only")]
        [HttpGet("adminOnly")]
        public async Task<IActionResult> AdminOnly()
        {   
            return Ok("You are admin");
        }


        [Authorize(Policy = "HR-only")]
        [HttpGet("hrOnly")]
        public async Task<IActionResult> HrOnly()
        {
            return Ok("hr");
        }

        [Authorize(Policy = "HR-Manager")]
        [HttpGet("hrManager")]
        public async Task<IActionResult> HrManager()
        {
            var name = User.Identity!.Name;
            return Ok($"HR Manager! {name}");
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("cookieAuth");
            return Ok("Sign Out ");
        }


        [Authorize(Policy = "over-18")]
        [HttpGet("18")]
        public async Task<IActionResult> Over18()
        {
            return Ok("18");
        }
    }
}
