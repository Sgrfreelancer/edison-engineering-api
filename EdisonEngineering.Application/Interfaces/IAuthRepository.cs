using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface IAuthRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
}