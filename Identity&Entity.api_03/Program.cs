using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


const string AuthSchema = "cookies";

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(AuthSchema)
    .AddCookie(AuthSchema);


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("role admin", p =>
    {
        p.RequireAuthenticatedUser() 
        //.AddAuthenticationSchemes(AuthSchema)
        .RequireClaim("role","admin");
    });
});

var app = builder.Build();  

// Configure the HTTP request pipeline.
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
