using Identity_Entity.Data_02.Data;
using Identity_Entity.Data_02.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity_Entity.api_05.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection MapIdentity(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<JwtAuthDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:JWT")!))
                    };
                });


            services.AddAuthorization();

            return services;
        }
    }
}
