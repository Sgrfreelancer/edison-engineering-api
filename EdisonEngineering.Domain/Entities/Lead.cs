namespace EdisonEngineering.Domain.Entities;

public class Lead
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Phone { get; set; }

    public string? Email { get; set; }

    public string City { get; set; }           // Pune, Mumbai
    public string ServiceType { get; set; }    // Solar / Electrical

    public string? Message { get; set; }
    public string Source { get; set; } = "Website"; // Contact / Calculator / Home

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}