using EdisonEngineering.Application.DTOs;

public interface IBlogService
{
    Task<List<BlogListDto>> GetAllAsync();
    Task<BlogDto?> GetBySlugAsync(string slug);
}