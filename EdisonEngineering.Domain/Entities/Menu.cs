namespace EdisonEngineering.Domain.Entities;

public class Menu
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Slug { get; set; }

    public int? ParentId { get; set; }

    public Menu Parent { get; set; }

    public ICollection<Menu> Children { get; set; }
}