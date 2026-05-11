using Identity_Entity.api_03.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity_Entity.api_03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [AllowAnonymous]
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


        [Authorize]
        [HttpGet("isAuth")]
        public async Task<IActionResult> IsAuth()
        {
            //if (!HttpContext.User.HasClaim("email",email))
            //{
            //    return StatusCode(401);
            //}
            return Ok(HttpContext.User.FindFirst("email")!.Value);
        }

        [Authorize(Policy = "role admin")]
        [HttpGet("isAdmin")]
        public async Task<IActionResult> IsAdminByPolicy()
        {
            //if (!HttpContext.User.HasClaim("role", "admin"))
            //{
            //    return StatusCode(401);
            //}
            return Ok(HttpContext.User.FindFirst("role")!.Value);
        }

        [Authorize(Roles ="admin")]
        [HttpGet("isRole")]
        public async Task<IActionResult> IsAdminByRole()
        {
            //if (!HttpContext.User.HasClaim("role", "admin"))
            //{
            //    return StatusCode(401);
            //}
            return Ok(HttpContext.User.FindFirst("role")!.Value);
        }

    }

}
