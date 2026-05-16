using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using EdisonEngineering.API.Auth;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BlogsController : ControllerBase
{
    private readonly IBlogService _service;

    private readonly ILogger<BlogsController> _logger;

    public BlogsController(
        IBlogService service,
        ILogger<BlogsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // =========================================================
    // GET ALL BLOGS
    // =========================================================

    [OutputCache(
        PolicyName = "blogs-cache")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] BlogQueryDto query)
    {
        _logger.LogInformation(
            "Fetching paginated blogs");

        var data =
            await _service.GetAllAsync(query);

        return Ok(
            new ApiResponse<PagedResponse<BlogListDto>>
            {
                Success = true,
                Message = "Blogs fetched successfully",
                Data = data
            });
    }

    // =========================================================
    // GET BLOG BY SLUG
    // =========================================================

    [OutputCache(
        PolicyName = "blogs-cache")]
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(
        string slug)
    {
        _logger.LogInformation(
            "Fetching blog by slug: {Slug}",
            slug);

        var data =
            await _service.GetBySlugAsync(slug);

        if (data == null)
        {
            _logger.LogWarning(
                "Blog not found for slug: {Slug}",
                slug);

            return NotFound(
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "Blog not found"
                });
        }

        return Ok(new ApiResponse<BlogDto>
        {
            Success = true,
            Message = "Blog fetched successfully",
            Data = data
        });
    }

    // =========================================================
    // CREATE BLOG
    // =========================================================

    [HasPermission("blog.create")]

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBlogDto dto)
    {
        _logger.LogInformation(
            "Creating blog with slug: {Slug}",
            dto.Slug);

        await _service.CreateAsync(dto);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Blog created successfully"
        });
    }

    // =========================================================
    // UPDATE BLOG
    // =========================================================

    [HasPermission("blog.edit")]

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateBlogDto dto)
    {
        _logger.LogInformation(
            "Updating blog with Id: {Id}",
            id);

        await _service.UpdateAsync(id, dto);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Blog updated successfully"
        });
    }

    // =========================================================
    // DELETE BLOG
    // =========================================================

    [HasPermission("blog.delete")]

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        _logger.LogInformation(
            "Deleting blog with Id: {Id}",
            id);

        await _service.DeleteAsync(id);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Blog deleted successfully"
        });
    }
}