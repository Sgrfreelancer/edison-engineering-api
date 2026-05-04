namespace EdisonEngineering.Domain.Entities;

public class ServiceCategory
{
    public int Id { get; set; }

    public string Name { get; set; }        // Solar, Electrical
    public string Slug { get; set; }        // solar, electrical

    public ICollection<Service> Services { get; set; }
}