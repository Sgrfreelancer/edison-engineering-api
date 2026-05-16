namespace EdisonEngineering.Application.DTOs;

public class LoginResponseDto
{
    public required string Token { get; set; }

    // ✅ NEW
    public required string RefreshToken { get; set; }

    // ✅ NEW
    public DateTime Expiration { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Role { get; set; }
}
