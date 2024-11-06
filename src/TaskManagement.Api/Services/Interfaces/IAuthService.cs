using TaskManagement.Api.Dtos;
using TaskManagement.Api.Dtos.Authentication;

namespace TaskManagement.Api.Services.Interfaces;

public interface IAuthService
{
    ApiResponse<string> GenerateToken(GenerateTokenRequest request);
    Task<ApiResponse<string>> SignUpAsync(SignUpRequest request);
    Task<ApiResponse<string>> LogInAsync(LogInRequest request);
}