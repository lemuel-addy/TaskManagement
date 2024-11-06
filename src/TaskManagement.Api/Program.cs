using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repositories.Interfaces;
using TaskManagement.Api.Repositories.Providers;
using TaskManagement.Api.Services.Interfaces;
using TaskManagement.Api.Services.Providers;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;


services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DbConnection")));

services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IAuthRepository, AuthRepository>();
services.Configure<BearerTokenConfig>(config.GetSection(nameof(BearerTokenConfig)));

services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["BearerTokenConfig:Issuer"],
                ValidAudience = builder.Configuration["BearerTokenConfig:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["BearerTokenConfig:SigningKey"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,         
                ValidateLifetime = true
            };
        }
    );
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();