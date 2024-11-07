using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBearerAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Action<BearerTokenConfig> bearerTokenConfigAction = bearerTokenConfig =>
            configuration.GetSection(nameof(BearerTokenConfig)).Bind(bearerTokenConfig);
        var bearerConfig = new BearerTokenConfig();
        bearerTokenConfigAction.Invoke(bearerConfig);
        
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
                        ValidIssuer = bearerConfig.Issuer,
                        ValidAudience = bearerConfig.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerConfig.SigningKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,         
                        ValidateLifetime = true
                    };
                }
            );
        return services;
    }
}