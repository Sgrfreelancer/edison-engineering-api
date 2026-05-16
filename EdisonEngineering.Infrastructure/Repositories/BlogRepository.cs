using EdisonEngineering.Application.Interfaces;
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
    GetPagedAsync(
        int page,
        int pageSize,
        string? search)
    {
        var query = _context.Blogs
            .AsNoTracking()
            .AsQueryable();

        // ✅ SEARCH

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Title.Contains(search));
        }

        // ✅ TOTAL COUNT

        var totalCount =
            await query.CountAsync();

        // ✅ PAGINATION

        var blogs = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (blogs, totalCount);
    }
}