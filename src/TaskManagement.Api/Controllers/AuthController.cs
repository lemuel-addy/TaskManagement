using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Dtos.Authentication;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<object>))]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("generate")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(GenerateToken), OperationId = nameof(GenerateToken))]
    public IActionResult GenerateToken([FromBody] GenerateTokenRequest request)
    {
        var result = _authService.GenerateToken(request);
        return StatusCode(result.Code, result);
    }
    
    [HttpPost]
    [Route("sign-up")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(SignUpAsync), OperationId = nameof(SignUpAsync))]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
    {
        var result = await _authService.SignUpAsync(request);
        return StatusCode(result.Code, result);
    }
    
    [HttpPost]
    [Route("log-in")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<object>))]
    [SwaggerOperation(nameof(LogInAsync), OperationId = nameof(LogInAsync))]
    public async Task<IActionResult> LogInAsync([FromBody] LogInRequest request)
    {
        var result = await _authService.LogInAsync(request);
        return StatusCode(result.Code, result);
    }
    


    // [HttpPost("token")]
    // public string Generate([FromBody] User user)
    // {
    //     string token = _authenticationService.GenerateToken(user.Contacts, user);
    //     return token;
    // }
    //
    // [HttpPost("login")]
    // public async Task<IActionResult> LoginAsync([FromBody] UserDto dto)
    // {
    //     User user = new()
    //     {
    //         Email = dto.Email,
    //         Password = dto.Password
    //     };
    //
    //         
    //
    //
    //     string tokenResult = await _userAccountService.LoginAsync(user);
    //
    //     return Ok(tokenResult);
    // }
    //
    // [HttpPost("register")]
    // public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto dto)
    // {
    //     //var encrypted = _hasher.VerifyHashedPassword(user,user.Password,dto.Password)
    //
    //     User user = new()
    //     {
    //         Username = dto.Username,
    //         Contacts = dto.Contacts,
    //         Email = dto.Email,
    //         Password = dto.Password,
    //         Role = new Role
    //         {
    //             UserRole = dto.Role.UserRole
    //         }
    //
    //     };
    //     user.Password = _hasher.HashPassword(user, user.Password);
    //     string tokenResult = await _userAccountService.RegisterAsync(user);
    //     return Ok(tokenResult);
    // }
}