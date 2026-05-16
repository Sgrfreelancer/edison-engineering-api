using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EdisonEngineering.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceCategory>> GetAllWithServicesAsync()
    {
        return await _context.ServiceCategories
            .Include(x => x.Services)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ServiceCategory?> GetBySlugAsync(string slug)
    {
        return await _context.ServiceCategories
            .Include(x => x.Services)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<Service?> GetServiceAsync(string categorySlug, string serviceSlug)
    {
        return await _context.Services
            .Include(x => x.ServiceCategory)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Slug == serviceSlug &&
                x.ServiceCategory.Slug == categorySlug);
    }
}