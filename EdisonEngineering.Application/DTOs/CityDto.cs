public class CityDto
{
    public string Name { get; set; }
    public string Slug { get; set; }

    public string Description { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }

    public List<ProjectDto> Projects { get; set; } = new();
}