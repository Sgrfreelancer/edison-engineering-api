using EdisonEngineering.Domain.Entities;

public interface IJobRepository
{
    Task<List<Job>> GetAllActiveAsync();
}