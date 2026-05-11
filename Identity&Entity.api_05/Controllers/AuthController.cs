using Identity_Entity.api_05.Models.Auth;
using Identity_Entity.api_05.Services;
using Identity_Entity.Data_02.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Entity.api_05.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(AuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            var token = await _authService.LoginAsync(requestModel);
            if (token is null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }


        [Authorize(Roles ="user")]
        [HttpGet("only-user")]
        public async Task<IActionResult> OnlyUser()
        {
            //var authUser = await _userManager.Users.ToList();
            return Ok("user");
        }
    }
}
