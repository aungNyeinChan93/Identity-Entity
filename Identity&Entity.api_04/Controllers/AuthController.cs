using Identity_Entity.api_04.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity_Entity.api_04.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDataProtectionProvider _ipd;

        public AuthController(IDataProtectionProvider ipd)
        {
            _ipd = ipd;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var protector = _ipd.CreateProtector("auth-protector");
            HttpContext.Response.Headers.SetCookie = $"email={protector.Protect(loginRequestModel.Email)}";

            return Ok("Login success");
        }

        [HttpGet("isAuth")]
        public async Task<IActionResult> IsAuth([FromQuery]string email)
        {
            var e = HttpContext.User.FindFirst("email")!.Value;
            if (!HttpContext.User.HasClaim("email",email))
            {
                return Unauthorized();
            }
            return Ok(e);
        }


        [HttpPost("buildInLogin")]
        public async Task<IActionResult> BuildInLogin([FromBody]LoginRequestModel request)
        {
            var claims = new ClaimsIdentity([
                    new Claim("email",request.Email),
                    new Claim("class",request.Class),
                ],"authWithEmail");
            var user = new ClaimsPrincipal(claims);

            await HttpContext.SignInAsync("authWithEmail",user);

            return Ok("Login success");
        }

        [Authorize(Policy ="class")]
        [HttpGet("isAuthByBuildIn")]
        public async Task<IActionResult> IsAuthByBuildIn()
        {
            //if (HttpContext.User.HasClaim("email",email))
            //{
            //    return Ok("Is Auth");
            //}
            return Ok("Is Class A");
        }




    }
}
