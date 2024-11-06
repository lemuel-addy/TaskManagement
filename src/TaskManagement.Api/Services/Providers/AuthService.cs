using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Dtos.Authentication;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repositories.Interfaces;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Services.Providers;

public class AuthService:IAuthService
{
    private readonly BearerTokenConfig _bearerTokenConfig;
    private readonly ILogger<AuthService> _logger;
    private readonly IAuthRepository _authRepository;
    
    public AuthService(IOptions<BearerTokenConfig> bearerTokenConfig, ILogger<AuthService> logger, IAuthRepository authRepository)
    {
        _bearerTokenConfig = bearerTokenConfig.Value;
        _logger = logger;
        _authRepository = authRepository;
    }
    public ApiResponse<string> GenerateToken(GenerateTokenRequest request)
    {
        var symmetricKey = Encoding.ASCII.GetBytes(_bearerTokenConfig.SigningKey);
        var now = DateTime.UtcNow;

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, request.Username)

            // new Claim(ClaimTypes.MobilePhone, request.)
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),

            SecurityAlgorithms.HmacSha256Signature);

        var jwt = new JwtSecurityToken(

            issuer: _bearerTokenConfig.Issuer,

            audience: _bearerTokenConfig.Audience,

            expires: now.AddHours(Convert.ToInt32(24)),

            signingCredentials: signingCredentials,

            claims: claims

        );
        return new ApiResponse<string> {Data =  new JwtSecurityTokenHandler().WriteToken(jwt) , Code = (int)HttpStatusCode.OK, IsSuccessful = true};
    }

    public Task<ApiResponse<string>> SignUpAsync(SignUpRequest request)
    {
        throw new NotImplementedException();
    }
    
    
    public Task<ApiResponse<string>> LogInAsync(LogInRequest request)
    {
        
        throw new NotImplementedException();
    }
}