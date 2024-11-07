using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Api.Data.Entities;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Dtos.Authentication;
using TaskManagement.Api.Models;
using TaskManagement.Api.Repositories.Interfaces;
using TaskManagement.Api.Services.Interfaces;
#nullable disable
namespace TaskManagement.Api.Services.Providers;

public class AuthService:IAuthService
{
    private readonly BearerTokenConfig _bearerTokenConfig;
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IAuthRepository _authRepository;
    
    public AuthService(IOptions<BearerTokenConfig> bearerTokenConfig, ILogger<AuthService> logger,IPasswordHasher<User> hasher, IAuthRepository authRepository)
    {
        _bearerTokenConfig = bearerTokenConfig.Value;
        _logger = logger;
        _authRepository = authRepository;
        _hasher = hasher;
    }
    public ApiResponse<string> GenerateToken(GenerateTokenRequest request)
    {
        _logger.LogDebug("[GenerateToken] Generating Token for {Username}", request.Username);
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
        
        _logger.LogDebug("[GenerateToken] Token Generated Successfully");
        return new ApiResponse<string> {Data =  new JwtSecurityTokenHandler().WriteToken(jwt) , Code = (int)HttpStatusCode.OK, IsSuccessful = true};
    }

    public async Task<ApiResponse<string>> SignUpAsync(SignUpRequest request)
    {
        try
        {
            _logger.LogDebug("[SignUpAsync] Request {Username}", request.Username);
            var userByEmail = await _authRepository.CheckUserByEmailAsync(request.Email);
            if (userByEmail)
            {
                _logger.LogDebug("[SignUpAsync] {Email} already exists", request.Email);
                return new ApiResponse<string>
                {
                    Data = "User with this email already exists", Code = (int)HttpStatusCode.BadRequest,
                    IsSuccessful = false
                };
            }

            var userByUsername = await _authRepository.CheckUserByUsernameAsync(request.Username);
            if (userByUsername)
            {
                _logger.LogDebug("[SignUpAsync] {Username} already exists", request.Username);
                return new ApiResponse<string>
                {
                    Data = "This username already exists", Code = (int)HttpStatusCode.BadRequest, IsSuccessful = false
                };
            }

            User user = new()
            {
                Username = request.Username,
                Contacts = request.Contacts,
                Email = request.Email,
                Password = request.Password

            };
            user.Password = _hasher.HashPassword(user, user.Password);
            
            await _authRepository.CreateUserAsync(user);
            
            var tokenRequest = new GenerateTokenRequest{Username = user.Username};
            var token = GenerateToken(tokenRequest);
            
            _logger.LogDebug("[SignUpAsync] User signed up successfully");
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogDebug("[SignUpAsync] Exception: {Message}", ex.Message);
            return new ApiResponse<string> { Data = "Something went wrong!",Code = (int)HttpStatusCode.InternalServerError, IsSuccessful = false} ;
        }
        
    }
    
    
    public async Task<ApiResponse<string>> LogInAsync(LogInRequest request)
    {
        try
        {
            _logger.LogDebug("[LogInAsync] Request {Username}", request.Username );
            var user = await _authRepository.GetUserByUsernameAsync(request.Username);

            if (user == null)
            {
                _logger.LogDebug("[LogInAsync] {Username} not found", request.Username);
                return new ApiResponse<string> { Data = "User Not Found! Sign Up first", Code = (int)HttpStatusCode.NotFound, IsSuccessful = false };
            }

            var verification = _hasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (verification.Equals(PasswordVerificationResult.Failed))
            {
                _logger.LogDebug("[LogInAsync] Invalid password for {Username}", request.Username);
                return new ApiResponse<string>
                    { Data = "Invalid Credentials Try Again", Code = (int)HttpStatusCode.BadRequest, IsSuccessful = false };
            }
            
            var tokenRequest = new GenerateTokenRequest{Username = user.Username};
            var token = GenerateToken(tokenRequest);
            
            _logger.LogDebug("[LogInAsync] Logged in successfully");
            return token;

        }
        catch(Exception ex)
        {
            _logger.LogDebug("[LogInAsync] Exception: {Message}", ex.Message);
            return new ApiResponse<string> { Data = "Something went wrong!",Code = (int)HttpStatusCode.InternalServerError, IsSuccessful = false} ;
        }
    }
}