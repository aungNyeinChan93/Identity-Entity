using Identity_Entity.api_04.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDataProtection();
builder.Services.AddScoped<IsAuthenticateMiddleware>();

builder.Services.AddAuthentication("authWithEmail")
    .AddCookie("authWithEmail");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("class", p =>
    {
        p.RequireAuthenticatedUser()
        .AddAuthenticationSchemes()
        .RequireClaim("class", "A");
    });
});

//builder.Services.a

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.UseMiddleware();

app.UseHttpsRedirection();

app.UseMiddleware<IsAuthenticateMiddleware>();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
