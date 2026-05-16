using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

namespace EdisonEngineering.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableRateLimiting("fixed")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _service;
    private readonly ILogger<CitiesController> _logger;

    public CitiesController(
        ICityService service,
        ILogger<CitiesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /api/cities
    [OutputCache(
        PolicyName = "cities-cache")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all cities");

        var data = await _service.GetAllAsync();

        if (data == null || !data.Any())
        {
            _logger.LogWarning("No cities found");

            return NotFound(new ApiResponse<List<CityDto>>
            {
                Success = false,
                Message = "No cities found",
                Data = new List<CityDto>()
            });
        }

        _logger.LogInformation(
            "Cities fetched successfully. Count: {Count}",
            data.Count());

        return Ok(new ApiResponse<IEnumerable<CityDto>>
        {
            Success = true,
            Message = "Cities fetched successfully",
            Data = data
        });
    }

    // GET: /api/cities/pune
    [OutputCache(
        PolicyName = "cities-cache")]
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        _logger.LogInformation(
            "Fetching city by slug: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("City slug was empty");

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
                "City not found for slug: {Slug}",
                slug);

            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "City not found"
            });
        }

        _logger.LogInformation(
            "City fetched successfully for slug: {Slug}",
            slug);

        return Ok(new ApiResponse<CityDto>
        {
            Success = true,
            Message = "City fetched successfully",
            Data = data
        });
    }

    // GET: /api/cities/pune/projects
    [OutputCache(
        PolicyName = "cities-cache")]
    [HttpGet("{slug}/projects")]
    public async Task<IActionResult> GetProjects(string slug)
    {
        _logger.LogInformation(
            "Fetching projects for city: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("City slug was empty for projects");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Slug is required"
            });
        }

        var data = await _service.GetProjectsAsync(slug);

        if (data == null || !data.Any())
        {
            _logger.LogWarning(
                "No projects found for city: {Slug}",
                slug);

            return NotFound(new ApiResponse<List<ProjectDto>>
            {
                Success = false,
                Message = "No projects found",
                Data = new List<ProjectDto>()
            });
        }

        _logger.LogInformation(
            "Projects fetched successfully for city: {Slug}",
            slug);

        return Ok(new ApiResponse<IEnumerable<ProjectDto>>
        {
            Success = true,
            Message = "Projects fetched successfully",
            Data = data
        });
    }
}
