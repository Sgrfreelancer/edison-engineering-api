namespace EdisonEngineering.Domain.Entities;

public class Job
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Department { get; set; } // Solar / Electrical
    public string Location { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}