using EdisonEngineering.Application.DTOs;

public interface ICityService
{
    Task<List<CityDto>> GetAllAsync();
    Task<CityDto?> GetBySlugAsync(string slug);
    Task<List<ProjectDto>> GetProjectsAsync(string slug);
}