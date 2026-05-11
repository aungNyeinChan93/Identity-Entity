using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity_Entity_api_02.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        private readonly IDataProtectionProvider idp;
        private readonly IHttpContextAccessor contextAccessor;

        public TestsController(IDataProtectionProvider idp, IHttpContextAccessor contextAccessor)
        {
            this.idp = idp;
            this.contextAccessor = contextAccessor;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            //var protector = idp.CreateProtector("auth-cookie-protector");
            //HttpContext.Response.Headers.SetCookie = $"auth={protector.Protect("name:chan")}";

            var claims = new ClaimsIdentity([
                    new Claim("name","aung"),
                    new Claim("passport","mm"),
                ], "cookie");

            var user = new ClaimsPrincipal(claims);

            await HttpContext.SignInAsync("cookie", user);

            return Ok("login"); 
        }

        [HttpGet("username")]
        public async Task<IActionResult> GetCookie()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return StatusCode(401);
            }

            if (!HttpContext.User.HasClaim("passport","mm"))
            {
                return StatusCode(403);
            }

            var claim = HttpContext.User.Claims.First().Value;
            var authName = HttpContext.User.FindFirst("name")!.Value;
            return Ok(new {claim,authName});
            //var isKeySuccess = HttpContext.Items.TryGetValue("AuthKey", out var key);
            //var isValueSuccess = HttpContext.Items.TryGetValue("AuthValue", out var value);
            //if (!isKeySuccess || !isValueSuccess)
            //{
            //    return Unauthorized();
            //}
            //return Ok(new
            //{
            //    key,value
            //});
        }
    }
}
