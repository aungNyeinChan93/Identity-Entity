using Identity_Entity.api_06.Authorization;
using Identity_Entity.api_06.Data;
using Identity_Entity.api_06.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthentication("cookieAuth")
    .AddCookie("cookieAuth", options =>
    {
        options.Cookie.Name = "cookieAuth";
        options.LoginPath = "/api/Account/login";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin-only", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "admin");
        //p.AddAuthenticationSchemes("cookieAuth")
        //.RequireAuthenticatedUser()

    });
    options.AddPolicy("HR-only", p =>
    {
        p.RequireClaim("dept", "HR");
    });

    options.AddPolicy("HR-Manager", p =>
    {
        p.RequireClaim("dept", "HR");
        p.RequireClaim(ClaimTypes.Role, "admin");
    });

    options.AddPolicy("over-18", p =>
    {
        p.Requirements.Add(new TestAuthorizationRequrement(18));
    });
});

builder.Services.AddScoped<AccountService>();
builder.Services.AddSingleton<IAuthorizationHandler, TestAuthorizationRequrementHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
