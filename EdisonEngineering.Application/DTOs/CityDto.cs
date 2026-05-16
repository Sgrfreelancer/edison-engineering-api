public class CityDto
{
    public required string Name { get; set; }
    public required string Slug { get; set; }

    public required string Description { get; set; }
    public required string ContactNumber { get; set; }
    public required string Email { get; set; }

    public List<ProjectDto> Projects { get; set; } = new();
}
