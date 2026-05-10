using Identity_Entity.api.Models.Auth;
using Identity_Entity.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Entity.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestModel requestModel)
        {
            var result = await _authService.RegisterAsync(requestModel);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var token = await _authService.LoginAsync(request);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TestAuth()
        {
            return Ok("Authrnticated");
        }
    }
}
