namespace EdisonEngineering.Domain.Entities;

public class City
{
    public int Id { get; set; }

    public string Name { get; set; }        // Pune
    public string Slug { get; set; }        // pune

    public string Description { get; set; } // SEO content

    public string ContactNumber { get; set; }
    public string Email { get; set; }

    public ICollection<Project> Projects { get; set; }
}