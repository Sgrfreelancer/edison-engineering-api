using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class SubsidyRepository : ISubsidyRepository
{
    private readonly AppDbContext _context;

    public SubsidyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Subsidy?> GetByKWAsync(decimal kw)
    {
        return await _context.Subsidies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => kw >= x.MinKW && kw <= x.MaxKW);
    }
}