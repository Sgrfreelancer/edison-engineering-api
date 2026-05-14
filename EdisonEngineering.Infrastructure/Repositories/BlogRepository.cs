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
}