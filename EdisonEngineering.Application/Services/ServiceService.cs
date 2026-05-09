using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _repo;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        IServiceRepository repo,
        ILogger<ServiceService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<ServiceCategoryDto>> GetAllAsync()
    {
        _logger.LogInformation(
            "Fetching all service categories");

        var data = await _repo.GetAllWithServicesAsync();

        if (data == null || !data.Any())
        {
            _logger.LogWarning(
                "No service categories found");

            return new List<ServiceCategoryDto>();
        }

        return data.Select(c => new ServiceCategoryDto
        {
            Name = c.Name,
            Slug = c.Slug,
            Services = c.Services.Select(s => new ServiceDto
            {
                Name = s.Name,
                Slug = s.Slug,
                Description = s.Description
            }).ToList()
        }).ToList();
    }

    public async Task<ServiceCategoryDto?> GetBySlugAsync(string slug)
    {
        _logger.LogInformation(
            "Fetching service category for slug: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning(
                "Service category slug is empty");

            return null;
        }

        var c = await _repo.GetBySlugAsync(slug);

        if (c == null)
        {
            _logger.LogWarning(
                "Service category not found for slug: {Slug}",
                slug);

            return null;
        }

        return new ServiceCategoryDto
        {
            Name = c.Name,
            Slug = c.Slug,
            Services = c.Services.Select(s => new ServiceDto
            {
                Name = s.Name,
                Slug = s.Slug,
                Description = s.Description
            }).ToList()
        };
    }

    public async Task<ServiceDto?> GetServiceAsync(
        string categorySlug,
        string serviceSlug)
    {
        _logger.LogInformation(
            "Fetching service details for category: {CategorySlug}, service: {ServiceSlug}",
            categorySlug,
            serviceSlug);

        if (string.IsNullOrWhiteSpace(categorySlug) ||
            string.IsNullOrWhiteSpace(serviceSlug))
        {
            _logger.LogWarning(
                "Category slug or service slug is empty");

            return null;
        }

        var s = await _repo.GetServiceAsync(
            categorySlug,
            serviceSlug);

        if (s == null)
        {
            _logger.LogWarning(
                "Service not found for category: {CategorySlug}, service: {ServiceSlug}",
                categorySlug,
                serviceSlug);

            return null;
        }

        return new ServiceDto
        {
            Name = s.Name,
            Slug = s.Slug,
            Description = s.Description
        };
    }
}