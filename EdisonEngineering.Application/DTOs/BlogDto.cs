public class BlogDto
{
    public string Title { get; set; }
    public string Slug { get; set; }

    public string Content { get; set; }

    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}