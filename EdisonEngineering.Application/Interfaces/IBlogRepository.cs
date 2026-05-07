using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface IBlogRepository
{
    Task<List<Blog>> GetAllAsync();
    Task<Blog?> GetBySlugAsync(string slug);
}