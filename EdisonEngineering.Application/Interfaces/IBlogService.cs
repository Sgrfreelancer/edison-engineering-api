using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;

public interface IBlogService
{
    Task<PagedResponse<BlogListDto>> GetAllAsync(
    BlogQueryDto query);
    Task<BlogDto?> GetBySlugAsync(string slug);

    // ✅ NEW METHODS

    Task CreateAsync(CreateBlogDto dto);

    Task UpdateAsync(int id, UpdateBlogDto dto);

    Task DeleteAsync(int id);
}