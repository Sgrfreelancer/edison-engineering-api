namespace EdisonEngineering.Domain.Entities;

public class Service
{
    public int Id { get; set; }

    public string Name { get; set; }        // Homes, Industrial
    public string Slug { get; set; }        // homes, industrial

    public string Description { get; set; }

    public int ServiceCategoryId { get; set; }
    public ServiceCategory ServiceCategory { get; set; }
}