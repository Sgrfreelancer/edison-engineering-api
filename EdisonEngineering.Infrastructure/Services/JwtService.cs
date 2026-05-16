using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using EdisonEngineering.Domain.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EdisonEngineering.Infrastructure.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // =====================================================
    // GENERATE JWT TOKEN
    // =====================================================

    public string GenerateToken(AppUser user)
    {
        var keyText = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "Jwt:Key is not configured.");

        var key = Encoding.UTF8.GetBytes(keyText);

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Name),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

        var credentials =
            new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

        var expires =
            DateTime.UtcNow.AddMinutes(15);

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "Jwt:Issuer is not configured.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "Jwt:Audience is not configured.");

        var token =
            new JwtSecurityToken(
                issuer: issuer,

                audience: audience,

                claims: claims,

                expires: expires,

                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    // =====================================================
    // GENERATE REFRESH TOKEN
    // =====================================================

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString()
             + Guid.NewGuid().ToString();
    }
}