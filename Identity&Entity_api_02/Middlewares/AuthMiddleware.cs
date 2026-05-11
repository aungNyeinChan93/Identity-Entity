using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

namespace Identity_Entity_api_02.Middlewares
{

    public class AuthMiddleware : IMiddleware
    {

        private readonly HashSet<string> _publicRoutes = new HashSet<string>()
        {
            "/",
            "/api/tests/login"
        }; 

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isPublicRoutes = _publicRoutes.Any(x=>x.Contains(context.Request.Path));

            if (isPublicRoutes)
            {
                goto skip;
            }

            var protector = context.RequestServices.GetRequiredService<IDataProtectionProvider>().CreateProtector("auth-cookie-protector");
            var authCookie = context.Request.Headers.Cookie.FirstOrDefault(x => x!.StartsWith("auth="));
            if (string.IsNullOrEmpty(authCookie)) return;

            var protectorPayload = authCookie.Split("=").Last();
            var payload = protector.Unprotect(protectorPayload);
            var parts = payload.Split(":");
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
