using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EdisonEngineering.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetAsync(string token)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        _context.RefreshTokens.Update(token);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
