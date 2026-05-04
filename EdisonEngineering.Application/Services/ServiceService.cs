using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _repo;

    public ServiceService(IServiceRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ServiceCategoryDto>> GetAllAsync()
    {
        var data = await _repo.GetAllWithServicesAsync();

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
        var c = await _repo.GetBySlugAsync(slug);
        if (c == null) return null;

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

    public async Task<ServiceDto?> GetServiceAsync(string categorySlug, string serviceSlug)
    {
        var s = await _repo.GetServiceAsync(categorySlug, serviceSlug);
        if (s == null) return null;

        return new ServiceDto
        {
            Name = s.Name,
            Slug = s.Slug,
            Description = s.Description
        };
    }
}