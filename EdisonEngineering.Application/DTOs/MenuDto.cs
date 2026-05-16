namespace EdisonEngineering.Application.DTOs;

public class MenuDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }

    public List<MenuDto> Children { get; set; } = new();
}
