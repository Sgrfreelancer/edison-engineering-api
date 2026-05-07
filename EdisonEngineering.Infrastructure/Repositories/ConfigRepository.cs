using Microsoft.EntityFrameworkCore;
using EdisonEngineering.Infrastructure.Persistence;

public class ConfigRepository : IConfigRepository
{
    private readonly AppDbContext _context;

    public ConfigRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, string>> GetAllAsync()
    {
        return await _context.AppConfigs
            .ToDictionaryAsync(x => x.Key, x => x.Value);
    }
}