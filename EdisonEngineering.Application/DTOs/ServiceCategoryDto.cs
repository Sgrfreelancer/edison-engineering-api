public class ServiceCategoryDto
{
    public string Name { get; set; }
    public string Slug { get; set; }

    public List<ServiceDto> Services { get; set; } = new();
}