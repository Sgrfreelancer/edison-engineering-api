using System.ComponentModel.DataAnnotations;

namespace EdisonEngineering.Application.DTOs;

public class UpdateBlogDto
{
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Slug { get; set; }

    [Required]
    public required string Content { get; set; }

    [MaxLength(200)]
    public string? MetaTitle { get; set; }

    [MaxLength(500)]
    public string? MetaDescription { get; set; }

    [Url]
    public string? ImageUrl { get; set; }
}
