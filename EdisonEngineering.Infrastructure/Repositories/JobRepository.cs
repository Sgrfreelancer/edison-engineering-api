using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;

    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Job>> GetAllActiveAsync()
    {
        return await _context.Jobs
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}