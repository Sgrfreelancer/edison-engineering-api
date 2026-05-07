namespace EdisonEngineering.Domain.Entities;

public class Project
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public int CityId { get; set; }
    public City City { get; set; }
}