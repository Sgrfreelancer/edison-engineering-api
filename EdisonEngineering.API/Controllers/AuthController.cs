using Asp.Versioning;

using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService service,
        ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // [HttpGet("generate-hash")]
    // public IActionResult GenerateHash()
    // {
    //     var hash =
    //         BCrypt.Net.BCrypt.HashPassword("Admin@123");

    //     return Ok(hash);
    // }

    /// <summary>
    /// Admin login API
    /// </summary>
    [EnableRateLimiting("login-policy")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto dto)
    {
        _logger.LogInformation(
            "Login API called for email: {Email}",
            dto.Email);

        var result =
            await _service.LoginAsync(dto);

        if (result == null)
        {
            _logger.LogWarning(
                "Invalid login attempt for email: {Email}",
                dto.Email);

            return Unauthorized(
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
        }

        // Add token expiration header to response
        Response.Headers.Append(
            "Token-Expiration",
            result.Expiration.ToString("O"));

        return Ok(
            new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = result
            });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
    {
        var result =
            await _service
                .RefreshTokenAsync(dto);

        if (result == null)
        {
            return Unauthorized(
                new ApiResponse<string>
                {
                    Success = false,
                    Message =
                        "Invalid or expired refresh token"
                });
        }

        // Add token expiration header to response
        Response.Headers.Append(
            "Token-Expiration",
            result.Expiration.ToString("O"));

        return Ok(
            new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message =
                    "Token refreshed successfully",

                Data = result
            });
    }
}
