using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces; 

public interface ISubsidyRepository
{
    Task<Subsidy?> GetByKWAsync(decimal kw);
}
