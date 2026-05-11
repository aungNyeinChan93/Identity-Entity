using Identity_Entity_api_02.Models.Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Entity_api_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDataProtectionProvider _idp;

        public AuthController(IDataProtectionProvider idp)
        {
            _idp = idp;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel request)
        {

            var protector = _idp.CreateProtector("auth-protector");

            //HttpContext.Response.Headers["Set-cookie"] = $"auth=email:{protector.Protect(request.Email)}";
            //HttpContext.Response.Headers["auth"] = $"auth=email:{protector.Protect(request.Email)}";

            HttpContext.Response.Headers.SetCookie = $"auth=email:{request.Email}";
            

            return Ok($"Login success with {request.Email}");

        }

        [HttpGet("isAuth")]
        public async Task<IActionResult> AuthUser([FromQuery]string email)
        {
            if (!HttpContext.User.HasClaim("email",email))
            {
                return StatusCode(401);
            }
            var e = HttpContext.User.FindFirst("email")!.Value;

            return Ok(e);
        }
    }
}
