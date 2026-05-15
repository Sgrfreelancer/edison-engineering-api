namespace EdisonEngineering.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? UserEmail { get; set; }

    public string Method { get; set; }

    public string Path { get; set; }

    public string? IpAddress { get; set; }

    public int StatusCode { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}
