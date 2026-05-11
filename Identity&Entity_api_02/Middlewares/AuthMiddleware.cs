using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

namespace Identity_Entity_api_02.Middlewares
{

    public class AuthMiddleware : IMiddleware
    {

        private readonly HashSet<string> protectedRoutes = new HashSet<string>()
        {
           "/api/Auth/isAuth"
        }; 

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!protectedRoutes.Contains(context.Request.Path))
            {
                await next(context);
                return;
            }

            //var protector = context.RequestServices
            //    .GetRequiredService<IDataProtectionProvider>()
            //    .CreateProtector("auth-protector");

            var authCookie = context.Request.Headers.Cookie.First();

            if (string.IsNullOrEmpty(authCookie)) return;

            var protectorPayload = authCookie.Split("=").Last() ;

            //var payload = protector.Unprotect(protectorPayload);
            var parts = protectorPayload.Split(":");
            var key = parts[0];
            var value = parts[1];

            //context.Items["AuthKey"] = key;
            //context.Items["AuthValue"] = value;

            var claims = new ClaimsIdentity(
            [
                new Claim(key,value),
            ]);

            context.User = new ClaimsPrincipal(claims);

        skip:
            await next(context);

        }
    }
}
