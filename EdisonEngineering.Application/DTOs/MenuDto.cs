namespace EdisonEngineering.Application.DTOs;

public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    public List<MenuDto> Children { get; set; } = new();
}