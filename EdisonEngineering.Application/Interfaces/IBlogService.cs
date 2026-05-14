using EdisonEngineering.Application.DTOs;

public interface IBlogService
{
    Task<List<BlogListDto>> GetAllAsync();
    Task<BlogDto?> GetBySlugAsync(string slug);

    // ✅ NEW METHODS

    Task CreateAsync(CreateBlogDto dto);

    Task UpdateAsync(int id, UpdateBlogDto dto);

    Task DeleteAsync(int id);
}