public class BlogDto
{
    public required string Title { get; set; }
    public required string Slug { get; set; }

    public required string Content { get; set; }

    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}
