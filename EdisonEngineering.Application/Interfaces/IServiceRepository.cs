using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface IServiceRepository
{
    Task<List<ServiceCategory>> GetAllWithServicesAsync();
    Task<ServiceCategory?> GetBySlugAsync(string slug);
    Task<Service?> GetServiceAsync(string categorySlug, string serviceSlug);
}
