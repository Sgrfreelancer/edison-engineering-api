using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.Application.Interfaces;

public interface IBlogRepository
{
    Task<List<Blog>> GetAllAsync();
    Task<Blog?> GetBySlugAsync(string slug);

    // ✅ NEW METHODS

    Task<Blog?> GetByIdAsync(int id);

    Task AddAsync(Blog blog);

    Task UpdateAsync(Blog blog);

    Task DeleteAsync(Blog blog);

    Task<(List<Blog> Blogs, int TotalCount)>
    GetPagedAsync(BlogFilterDto filter);
}