using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;

namespace EdisonEngineering.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableRateLimiting("fixed")]
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

    // GET: /api/blogs
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all blogs");

        var data = await _service.GetAllAsync();

        if (data == null || !data.Any())
        {
            _logger.LogWarning("No blogs found");

            return NotFound(new ApiResponse<List<BlogDto>>
            {
                Success = false,
                Message = "No blogs found",
                Data = new List<BlogDto>()
            });
        }

        _logger.LogInformation(
            "Blogs fetched successfully. Count: {Count}",
            data.Count());

        return Ok(new ApiResponse<List<BlogListDto>>
        {
            Success = true,
            Message = "Blogs fetched successfully",
            Data = data
        });
    }

    // GET: /api/blogs/{slug}
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        _logger.LogInformation(
            "Fetching blog by slug: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("Blog slug was empty");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Slug is required"
            });
        }

        var data = await _service.GetBySlugAsync(slug);

        if (data == null)
        {
            _logger.LogWarning(
                "Blog not found for slug: {Slug}",
                slug);

            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Blog not found"
            });
        }

        _logger.LogInformation(
            "Blog fetched successfully for slug: {Slug}",
            slug);

        return Ok(new ApiResponse<BlogDto>
        {
            Success = true,
            Message = "Blog fetched successfully",
            Data = data
        });
    }
}