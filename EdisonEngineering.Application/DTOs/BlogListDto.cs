public class BlogListDto
{
    public required string Title { get; set; }
    public required string Slug { get; set; }

    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
