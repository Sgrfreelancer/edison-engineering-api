using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface ILeadRepository
{
    Task AddAsync(Lead lead);
}