using Identity_Entity_api_02.Middlewares;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddDataProtection();
//builder.Services.AddScoped<AuthMiddleware>();

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 

//app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.Run();
