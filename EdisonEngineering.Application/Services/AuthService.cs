using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BCrypt.Net;

using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Microsoft.IdentityModel.Tokens;

namespace EdisonEngineering.Application.Services;


public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IRefreshTokenRepository
    _refreshTokenRepository;

    public AuthService(
        IAuthRepository repo,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _repo = repo;
        _configuration = configuration;
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
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

        var refreshToken =
            GenerateRefreshToken();

        var expiryMinutes =
            Convert.ToDouble(
                _configuration["Jwt:ExpiryMinutes"]);

        var expiration =
            DateTime.UtcNow.AddMinutes(
                expiryMinutes);

        await _refreshTokenRepository.AddAsync(
            new RefreshToken
            {
                UserId = user.Id,

                Token = refreshToken,

                ExpiryDate =
                    DateTime.UtcNow.AddDays(7),

                IsRevoked = false
            });

        await _refreshTokenRepository
            .SaveChangesAsync();
                
        _logger.LogInformation(
            "Login successful for email: {Email}",
            dto.Email);

        return new LoginResponseDto
        {
            Token = token,

            RefreshToken = refreshToken,

            Expiration = expiration,

            Name = user.Name,

            Email = user.Email,

            Role = user.Role
        };
    }

    public async Task<LoginResponseDto?>
        RefreshTokenAsync(
            RefreshTokenRequestDto dto)
    {
        _logger.LogInformation(
            "Refresh token request received");

        var existingToken =
            await _refreshTokenRepository
                .GetAsync(dto.RefreshToken);

        if (existingToken == null)
        {
            _logger.LogWarning(
                "Invalid refresh token");

            return null;
        }

        if (existingToken.IsRevoked)
        {
            _logger.LogWarning(
                "Revoked refresh token used");

            return null;
        }

        if (existingToken.ExpiryDate
            < DateTime.UtcNow)
        {
            _logger.LogWarning(
                "Expired refresh token used");

            return null;
        }

        var user =
            await _repo.GetByIdAsync(
                existingToken.UserId);

        if (user == null)
        {
            _logger.LogWarning(
                "User not found for refresh token");

            return null;
        }

        // ✅ REVOKE OLD TOKEN

        existingToken.IsRevoked = true;

        await _refreshTokenRepository
            .UpdateAsync(existingToken);

        // ✅ GENERATE NEW TOKENS

        var newJwt =
            GenerateJwtToken(user);

        var newRefreshToken =
            GenerateRefreshToken();

        var expiryMinutes =
            Convert.ToDouble(
                _configuration["Jwt:ExpiryMinutes"]);

        var expiration =
            DateTime.UtcNow.AddMinutes(
                expiryMinutes);

        await _refreshTokenRepository
            .AddAsync(
                new RefreshToken
                {
                    UserId = user.Id,

                    Token = newRefreshToken,

                    ExpiryDate =
                        DateTime.UtcNow.AddDays(7),

                    IsRevoked = false
                });

        await _refreshTokenRepository
            .SaveChangesAsync();

        _logger.LogInformation(
            "Refresh token generated successfully");

        return new LoginResponseDto
        {
            Token = newJwt,

            RefreshToken = newRefreshToken,

            Expiration = expiration,

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
    
    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString()
            + Guid.NewGuid().ToString();
    }
}