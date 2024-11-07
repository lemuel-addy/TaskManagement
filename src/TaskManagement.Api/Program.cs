using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Data.Entities;
using TaskManagement.Api.Extensions;
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

services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IAuthRepository, AuthRepository>();
services.Configure<BearerTokenConfig>(config.GetSection(nameof(BearerTokenConfig)));

services.AddBearerAuthentication(config);

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
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();