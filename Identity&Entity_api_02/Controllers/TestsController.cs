using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var protector = idp.CreateProtector("auth-cookie-protector");
            //HttpContext.Response.Headers.SetCookie = $"auth={protector.Protect("name:chan")}";
            contextAccessor.HttpContext!.Response.Headers.SetCookie = $"auth={protector.Protect("name:chan")}";
            return Ok("login");
        }

        [HttpGet("username")]
        public async Task<IActionResult> GetCookie()
        {
           
            var isKeySuccess = HttpContext.Items.TryGetValue("AuthKey", out var key);
            var isValueSuccess = HttpContext.Items.TryGetValue("AuthValue", out var value);
            if (!isKeySuccess || !isValueSuccess)
            {
                return Unauthorized();
            }
            return Ok(new
            {
                key,value
            });
        }
    }
}
