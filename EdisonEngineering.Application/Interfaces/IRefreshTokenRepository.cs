using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);

    Task<RefreshToken?> GetAsync(string token);

    Task UpdateAsync(RefreshToken token);

    Task SaveChangesAsync();
}