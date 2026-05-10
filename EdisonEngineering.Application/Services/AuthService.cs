using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BCrypt.Net;

using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Microsoft.IdentityModel.Tokens;

namespace EdisonEngineering.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IAuthRepository repo,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _repo = repo;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto dto)
    {
        _logger.LogInformation(
            "Login attempt for email: {Email}",
            dto.Email);

        var user = await _repo.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            _logger.LogWarning(
                "Login failed. User not found for email: {Email}",
                dto.Email);

            return null;
        }

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            _logger.LogWarning(
                "Invalid password attempt for email: {Email}",
                dto.Email);

            return null;
        }

        if (!user.IsActive)
        {
            _logger.LogWarning(
                "Inactive user login attempt: {Email}",
                dto.Email);

            return null;
        }

        var token = GenerateJwtToken(user);

        _logger.LogInformation(
            "Login successful for email: {Email}",
            dto.Email);

        return new LoginResponseDto
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }

    private string GenerateJwtToken(
        EdisonEngineering.Domain.Entities.AppUser user)
    {
        var jwtSettings =
            _configuration.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(ClaimTypes.Name,
                user.Name),

            new Claim(ClaimTypes.Email,
                user.Email),

            new Claim(ClaimTypes.Role,
                user.Role)
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtSettings["Key"]));

        var creds =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var expiryMinutes =
            Convert.ToDouble(
                jwtSettings["ExpiryMinutes"]);

        var token =
            new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],

                audience: jwtSettings["Audience"],

                claims: claims,

                expires:
                    DateTime.UtcNow.AddMinutes(
                        expiryMinutes),

                signingCredentials: creds
            );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}