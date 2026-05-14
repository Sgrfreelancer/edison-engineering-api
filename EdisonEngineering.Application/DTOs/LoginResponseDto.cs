namespace EdisonEngineering.Application.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; }

    // ✅ NEW
    public string RefreshToken { get; set; }

    // ✅ NEW
    public DateTime Expiration { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }
}