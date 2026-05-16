using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;

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

    // =========================================================
    // GET ALL BLOGS
    // =========================================================

    public async Task<PagedResponse<BlogListDto>>
        GetAllAsync(BlogQueryDto query)
    {
        _logger.LogInformation(
            "Fetching blogs. Page: {Page}, PageSize: {PageSize}, Search: {Search}",
            query.Page,
            query.PageSize,
            query.Search);

        var filter = new BlogFilterDto
        {
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = null,
            Descending = false
        };

        var (blogs, totalCount) =
            await _repo.GetPagedAsync(filter);

        var items = blogs.Select(b => new BlogListDto
        {
            Title = b.Title,
            Slug = b.Slug,
            ImageUrl = b.ImageUrl,
            CreatedAt = b.CreatedAt
        }).ToList();

        return new PagedResponse<BlogListDto>
        {
            Items = items,

            Page = query.Page,

            PageSize = query.PageSize,

            TotalCount = totalCount,

            TotalPages =
                (int)Math.Ceiling(
                    totalCount /
                    (double)query.PageSize)
        };
    }

    // =========================================================
    // GET BLOG BY SLUG
    // =========================================================

    public async Task<BlogDto?> GetBySlugAsync(string slug)
    {
        _logger.LogInformation(
            "Fetching blog by slug: {Slug}",
            slug);

        var b = await _repo.GetBySlugAsync(slug);

        if (b == null)
        {
            _logger.LogWarning(
                "Blog not found for slug: {Slug}",
                slug);

            return null;
        }

        return new BlogDto
        {
            Title = b.Title,
            Slug = b.Slug,
            Content = b.Content,
            MetaTitle = b.MetaTitle,
            MetaDescription = b.MetaDescription,
            ImageUrl = b.ImageUrl,
            CreatedAt = b.CreatedAt
        };
    }

    // =========================================================
    // CREATE BLOG
    // =========================================================

    public async Task CreateAsync(CreateBlogDto dto)
    {
        _logger.LogInformation(
            "Creating blog with slug: {Slug}",
            dto.Slug);

        // ✅ DUPLICATE SLUG CHECK

        var existingBlog =
            await _repo.GetBySlugAsync(dto.Slug);

        if (existingBlog != null)
        {
            _logger.LogWarning(
                "Duplicate slug detected: {Slug}",
                dto.Slug);

            throw new Exception(
                "Blog slug already exists");
        }

        var blog = new Blog
        {
            Title = dto.Title,
            Slug = dto.Slug,
            Content = dto.Content,
            MetaTitle = dto.MetaTitle,
            MetaDescription = dto.MetaDescription,
            ImageUrl = dto.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Admin"
        };

        await _repo.AddAsync(blog);

        _logger.LogInformation(
            "Blog created successfully with slug: {Slug}",
            dto.Slug);
    }

    // =========================================================
    // UPDATE BLOG
    // =========================================================

    public async Task UpdateAsync(
        int id,
        UpdateBlogDto dto)
    {
        _logger.LogInformation(
            "Updating blog with Id: {Id}",
            id);

        var blog =
            await _repo.GetByIdAsync(id);

        if (blog == null)
        {
            _logger.LogWarning(
                "Blog not found for update. Id: {Id}",
                id);

            throw new Exception(
                "Blog not found");
        }

        // ✅ CHECK DUPLICATE SLUG

        var existingSlug =
            await _repo.GetBySlugAsync(dto.Slug);

        if (existingSlug != null &&
            existingSlug.Id != id)
        {
            _logger.LogWarning(
                "Duplicate slug during update: {Slug}",
                dto.Slug);

            throw new Exception(
                "Blog slug already exists");
        }

        blog.Title = dto.Title;
        blog.Slug = dto.Slug;
        blog.Content = dto.Content;
        blog.MetaTitle = dto.MetaTitle;
        blog.MetaDescription = dto.MetaDescription;
        blog.ImageUrl = dto.ImageUrl;
        blog.UpdatedAt = DateTime.UtcNow;
        blog.UpdatedBy = "Admin";

        await _repo.UpdateAsync(blog);

        _logger.LogInformation(
            "Blog updated successfully. Id: {Id}",
            id);
    }

    // =========================================================
    // DELETE BLOG
    // =========================================================

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation(
            "Soft deleting blog with Id: {Id}",
            id);

        var blog =
            await _repo.GetByIdAsync(id);

        if (blog == null)
        {
            _logger.LogWarning(
                "Blog not found for deletion. Id: {Id}",
                id);

            throw new Exception(
                "Blog not found");
        }

        // ✅ SOFT DELETE

        blog.IsDeleted = true;

        blog.DeletedAt = DateTime.UtcNow;

        blog.DeletedBy = "Admin";

        await _repo.UpdateAsync(blog);

        _logger.LogInformation(
            "Blog soft deleted successfully. Id: {Id}",
            id);
    }

    // =========================================================
    // GET PAGED BLOGS WITH FILTER
    // =========================================================

    public async Task<PagedResponse<BlogListDto>>
    GetPagedAsync(BlogFilterDto filter)
    {
        _logger.LogInformation(
            "Fetching paged blogs with filter. Page: {Page}, PageSize: {PageSize}, Search: {Search}, SortBy: {SortBy}",
            filter.Page,
            filter.PageSize,
            filter.Search,
            filter.SortBy);

        var (blogs, totalCount) =
            await _repo.GetPagedAsync(filter);

        var items =
            blogs.Select(x =>
                new BlogListDto
                {
                    Title = x.Title,

                    Slug = x.Slug,

                    ImageUrl = x.ImageUrl,

                    CreatedAt = x.CreatedAt
                })
            .ToList();

        return new PagedResponse<BlogListDto>
        {
            Items = items,

            Page = filter.Page,

            PageSize = filter.PageSize,

            TotalCount = totalCount,

            TotalPages =
                (int)Math.Ceiling(
                    totalCount /
                    (double)filter.PageSize)
        };
    }
}