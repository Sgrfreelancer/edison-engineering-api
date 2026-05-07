using EdisonEngineering.Domain.Entities;

public interface ISlabRepository
{
    Task<List<ElectricitySlab>> GetAllAsync();
}