namespace EdisonEngineering.Application.DTOs;

public class BlogFilterDto
{
    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public bool Descending { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
