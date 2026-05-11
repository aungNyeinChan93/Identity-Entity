using Identity_Entity.api_03.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity_Entity.api_03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel requestModel)
        {
            var claims = new ClaimsIdentity(
                [
                    new Claim("email", requestModel.Email),
                    new Claim("role",requestModel.Role)
                ],"cookies");

            var user = new ClaimsPrincipal(claims);

           await HttpContext.SignInAsync("cookies",user);

           return Ok("Login Success");  
        }


        [HttpGet("isAuth")]
        public async Task<IActionResult> IsAuth([FromQuery]string email)
        {
            if (!HttpContext.User.HasClaim("email",email))
            {
                return StatusCode(401);
            }
            return Ok(HttpContext.User.FindFirst("email")!.Value);
        }

        [HttpGet("isAdmin")]
        public async Task<IActionResult> IsAdmin()
        {
            if (!HttpContext.User.HasClaim("role", "admin"))
            {
                return StatusCode(401);
            }
            return Ok(HttpContext.User.FindFirst("role")!.Value);
        }
    }

}
