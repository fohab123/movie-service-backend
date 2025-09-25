using movie_service_backend;
using movie_service_backend.Data;
using movie_service_backend.Startup;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Registracija tvojih servisa, repozitorijuma i DbContext-a
builder.Services.AddApplicationServices(builder.Configuration);

// Swagger (ako želiš)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Middleware konfiguracija (CORS, Auth, itd.)
builder.Services.ConfigureMiddleware();

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Pipeline konfiguracija
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
    app.UseAuthorization();

}

app.ConfigureMiddlewarePipeline();

app.Run();

