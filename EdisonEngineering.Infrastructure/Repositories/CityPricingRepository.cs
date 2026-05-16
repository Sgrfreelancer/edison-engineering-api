using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CityPricingRepository : ICityPricingRepository
{
    private readonly AppDbContext _context;

    public CityPricingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CityPricing?> GetByCityAsync(string city)
    {
        return await _context.CityPricings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.City == city);
    }
}