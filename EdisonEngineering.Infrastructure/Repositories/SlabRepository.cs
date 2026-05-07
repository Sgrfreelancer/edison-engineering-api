using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class SlabRepository : ISlabRepository
{
    private readonly AppDbContext _context;

    public SlabRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ElectricitySlab>> GetAllAsync()
    {
        return await _context.ElectricitySlabs
            .OrderBy(x => x.MinUnit)
            .ToListAsync();
    }
}