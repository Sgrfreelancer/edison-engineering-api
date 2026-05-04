using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface IMenuRepository
{
    Task<List<Menu>> GetAllAsync();
}