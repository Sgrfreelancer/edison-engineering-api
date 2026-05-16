using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EdisonEngineering.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly AppDbContext _context;

    public BlogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Blog>> GetAllAsync()
    {
        return await _context.Blogs
            .OrderByDescending(b => b.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Blog?> GetBySlugAsync(string slug)
    {
        return await _context.Blogs
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Slug == slug);
    }

    // ✅ NEW

    public async Task<Blog?> GetByIdAsync(int id)
    {
        return await _context.Blogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // ✅ NEW

    public async Task AddAsync(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);

        await _context.SaveChangesAsync();
    }

    // ✅ NEW

    public async Task UpdateAsync(Blog blog)
    {
        _context.Blogs.Update(blog);

        await _context.SaveChangesAsync();
    }

    // ✅ NEW

    public async Task DeleteAsync(Blog blog)
    {
        _context.Blogs.Remove(blog);

        await _context.SaveChangesAsync();
    }

    public async Task<(List<Blog> Blogs, int TotalCount)>
    GetPagedAsync(BlogFilterDto filter)
    {
        var query = _context.Blogs
            .AsNoTracking()
            .AsQueryable();

        // =====================================
        // SEARCH
        // =====================================

        if (!string.IsNullOrWhiteSpace(
            filter.Search))
        {
            query = query.Where(x =>
                x.Title.Contains(filter.Search));
        }

        // =====================================
        // SORTING
        // =====================================

        query =
            filter.SortBy?.ToLower() switch
            {
                "title" =>
                    filter.Descending
                        ? query.OrderByDescending(
                            x => x.Title)
                        : query.OrderBy(
                            x => x.Title),

                _ =>
                    filter.Descending
                        ? query.OrderByDescending(
                            x => x.CreatedAt)
                        : query.OrderBy(
                            x => x.CreatedAt)
            };

        // =====================================
        // TOTAL COUNT
        // =====================================

        var totalCount =
            await query.CountAsync();

        // =====================================
        // PAGINATION
        // =====================================

        var blogs =
            await query
                .Skip(
                    (filter.Page - 1)
                    * filter.PageSize)

                .Take(filter.PageSize)

                .ToListAsync();

        return (blogs, totalCount);
    }
}