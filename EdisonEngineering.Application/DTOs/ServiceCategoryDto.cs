public class ServiceCategoryDto
{
    public required string Name { get; set; }
    public required string Slug { get; set; }

    public List<ServiceDto> Services { get; set; } = new();
}
