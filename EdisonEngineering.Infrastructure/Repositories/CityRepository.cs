using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EdisonEngineering.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _context.Cities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<City?> GetBySlugAsync(string slug)
    {
        return await _context.Cities
            .Include(c => c.Projects)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<List<Project>> GetProjectsByCitySlugAsync(string slug)
    {
        return await _context.Projects
            .Include(p => p.City)
            .AsSplitQuery()
            .Where(p => p.City.Slug == slug)
            .AsNoTracking()
            .ToListAsync();
    }
}