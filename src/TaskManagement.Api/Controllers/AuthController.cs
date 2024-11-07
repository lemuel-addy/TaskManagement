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
}