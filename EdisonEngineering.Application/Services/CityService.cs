using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _repo;
    private readonly ILogger<CityService> _logger;

    public CityService(
        ICityRepository repo,
        ILogger<CityService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<CityDto>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all cities from repository");

        var data = await _repo.GetAllAsync();

        if (data == null || !data.Any())
        {
            _logger.LogWarning("No cities found in repository");

            return new List<CityDto>();
        }

        var result = data.Select(c => new CityDto
        {
            Name = c.Name,
            Slug = c.Slug,
            Description = c.Description,
            ContactNumber = c.ContactNumber,
            Email = c.Email,
            Projects = c.Projects?.Select(p => new ProjectDto
            {
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            }).ToList() ?? new()
        }).ToList();

        _logger.LogInformation(
            "Cities mapped successfully. Count: {Count}",
            result.Count);

        return result;
    }

    public async Task<CityDto?> GetBySlugAsync(string slug)
    {
        _logger.LogInformation(
            "Fetching city by slug: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("City slug was empty");

            return null;
        }

        var c = await _repo.GetBySlugAsync(slug);

        if (c == null)
        {
            _logger.LogWarning(
                "City not found for slug: {Slug}",
                slug);

            return null;
        }

        var result = new CityDto
        {
            Name = c.Name,
            Slug = c.Slug,
            Description = c.Description,
            ContactNumber = c.ContactNumber,
            Email = c.Email,
            Projects = c.Projects?.Select(p => new ProjectDto
            {
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            }).ToList() ?? new()
        };

        _logger.LogInformation(
            "City fetched successfully for slug: {Slug}",
            slug);

        return result;
    }

    public async Task<List<ProjectDto>> GetProjectsAsync(string slug)
    {
        _logger.LogInformation(
            "Fetching projects for city slug: {Slug}",
            slug);

        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("City slug was empty for project fetch");

            return new List<ProjectDto>();
        }

        var data = await _repo.GetProjectsByCitySlugAsync(slug);

        if (data == null || !data.Any())
        {
            _logger.LogWarning(
                "No projects found for city slug: {Slug}",
                slug);

            return new List<ProjectDto>();
        }

        var result = data.Select(p => new ProjectDto
        {
            Title = p.Title,
            Description = p.Description,
            ImageUrl = p.ImageUrl
        }).ToList();

        _logger.LogInformation(
            "Projects fetched successfully for city slug: {Slug}. Count: {Count}",
            slug,
            result.Count);

        return result;
    }
}