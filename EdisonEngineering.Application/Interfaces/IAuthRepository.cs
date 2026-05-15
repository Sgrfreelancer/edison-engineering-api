using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.Application.Interfaces;

public interface IAuthRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(int id);
}