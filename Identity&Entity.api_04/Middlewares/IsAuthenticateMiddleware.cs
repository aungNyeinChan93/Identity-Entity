using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

namespace Identity_Entity.api_04.Middlewares
{
    public class IsAuthenticateMiddleware : IMiddleware
    {
        private readonly HashSet<string> _protectedRoutes = new HashSet<string>
        {
            "/api/Auth/isAuth"
        };
           
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isProtectedRoute = _protectedRoutes.Contains(context.Request.Path);

            if (!isProtectedRoute)
            {
                goto skip;
            }

            var protector = context.RequestServices
                .GetRequiredService<IDataProtectionProvider>().CreateProtector("auth-protector");

            var authCookie = context.Request.Headers.Cookie.First(x => x!.StartsWith("email="));

            var payload = authCookie!.Split("=");
            var key = payload[0];
            var value = protector.Unprotect(payload[1]);

            var claims = new ClaimsIdentity(
            [
                new Claim(key,value)
            ]);

            var user = new ClaimsPrincipal(claims);

            context.User = user;

        skip:

            await next(context);
        }
    }
}
