using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _repo;
    private readonly ILogger<BlogService> _logger;

    public BlogService(
        IBlogRepository repo,
        ILogger<BlogService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<BlogListDto>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all blogs from repository");

        var data = await _repo.GetAllAsync();

        if (data == null || !data.Any())
        {
            _logger.LogWarning("No blogs found in repository");

            return new List<BlogListDto>();
        }

        var result = data.Select(b => new BlogListDto
        {
            Title = b.Title,
            Slug = b.Slug,
            ImageUrl = b.ImageUrl,
            CreatedAt = b.CreatedAt
        }).ToList();

        _logger.LogInformation(
            "Blogs mapped successfully. Count: {Count}",
            result.Count);

        return result;
    }

    public async Task<BlogDto?> GetBySlugAsync(string slug)
    {
        _logger.LogInformation(
            "Fetching blog by slug: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("Blog slug was empty");

            return null;
        }

        var b = await _repo.GetBySlugAsync(slug);

        if (b == null)
        {
            _logger.LogWarning(
                "Blog not found for slug: {Slug}",
                slug);

            return null;
        }

        var result = new BlogDto
        {
            Title = b.Title,
            Slug = b.Slug,
            Content = b.Content,
            MetaTitle = b.MetaTitle,
            MetaDescription = b.MetaDescription,
            ImageUrl = b.ImageUrl,
            CreatedAt = b.CreatedAt
        };

        _logger.LogInformation(
            "Blog fetched successfully for slug: {Slug}",
            slug);

        return result;
    }
}