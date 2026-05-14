namespace EdisonEngineering.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // OPTIONAL NAVIGATION
    public AppUser User { get; set; }
}