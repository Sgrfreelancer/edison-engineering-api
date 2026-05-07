namespace EdisonEngineering.Domain.Entities;

public class JobApplication
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }

    public string ResumeUrl { get; set; } // later file upload

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}