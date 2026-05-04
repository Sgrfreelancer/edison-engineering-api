using EdisonEngineering.Application.DTOs;

public interface IServiceService
{
    Task<List<ServiceCategoryDto>> GetAllAsync();
    Task<ServiceCategoryDto?> GetBySlugAsync(string slug);
    Task<ServiceDto?> GetServiceAsync(string categorySlug, string serviceSlug);
}