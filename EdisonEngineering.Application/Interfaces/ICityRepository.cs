using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface ICityRepository
{
    Task<List<City>> GetAllAsync();
    Task<City?> GetBySlugAsync(string slug);
    Task<List<Project>> GetProjectsByCitySlugAsync(string slug);
}