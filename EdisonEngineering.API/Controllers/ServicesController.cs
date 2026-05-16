using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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
public class ServicesController : ControllerBase
{
    private readonly IServiceService _service;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(
        IServiceService service,
        ILogger<ServicesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /api/services
    [OutputCache(
        PolicyName = "services-cache")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all service categories");

        var data = await _service.GetAllAsync();

        if (data == null || !data.Any())
        {
            _logger.LogWarning("No service categories found");

            return NotFound(new ApiResponse<List<ServiceCategoryDto>>
            {
                Success = false,
                Message = "No services found",
                Data = new List<ServiceCategoryDto>()
            });
        }

        _logger.LogInformation(
            "Service categories fetched successfully. Count: {Count}",
            data.Count());

        return Ok(new ApiResponse<IEnumerable<ServiceCategoryDto>>
        {
            Success = true,
            Message = "Services fetched successfully",
            Data = data
        });
    }

    // GET: /api/services/solar
    [OutputCache(
        PolicyName = "services-cache")]
    [HttpGet("{categorySlug}")]
    public async Task<IActionResult> GetByCategory(string categorySlug)
    {
        if (string.IsNullOrWhiteSpace(categorySlug))
        {
            _logger.LogWarning("Category slug was empty");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Category slug is required"
            });
        }

        _logger.LogInformation(
            "Fetching service category for slug: {CategorySlug}",
            categorySlug);

        var data = await _service.GetBySlugAsync(categorySlug);

        if (data == null)
        {
            _logger.LogWarning(
                "Service category not found for slug: {CategorySlug}",
                categorySlug);

            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Service category not found"
            });
        }

        _logger.LogInformation(
            "Service category fetched successfully for slug: {CategorySlug}",
            categorySlug);

        return Ok(new ApiResponse<ServiceCategoryDto>
        {
            Success = true,
            Message = "Service category fetched successfully",
            Data = data
        });
    }

    // GET: /api/services/solar/homes
    [OutputCache(
        PolicyName = "services-cache")]
    [HttpGet("{categorySlug}/{serviceSlug}")]
    public async Task<IActionResult> GetService(
        string categorySlug,
        string serviceSlug)
    {
        if (string.IsNullOrWhiteSpace(categorySlug) ||
            string.IsNullOrWhiteSpace(serviceSlug))
        {
            _logger.LogWarning(
                "Category slug or service slug was empty");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Category slug and service slug are required"
            });
        }

        _logger.LogInformation(
            "Fetching service details for category: {CategorySlug}, service: {ServiceSlug}",
            categorySlug,
            serviceSlug);

        var data = await _service.GetServiceAsync(
            categorySlug,
            serviceSlug);

        if (data == null)
        {
            _logger.LogWarning(
                "Service not found for category: {CategorySlug}, service: {ServiceSlug}",
                categorySlug,
                serviceSlug);

            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Service not found"
            });
        }

        _logger.LogInformation(
            "Service fetched successfully for category: {CategorySlug}, service: {ServiceSlug}",
            categorySlug,
            serviceSlug);

        return Ok(new ApiResponse<ServiceDto>
        {
            Success = true,
            Message = "Service fetched successfully",
            Data = data
        });
    }
}