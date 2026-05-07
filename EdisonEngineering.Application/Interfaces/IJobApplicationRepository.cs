using EdisonEngineering.Domain.Entities;

public interface IJobApplicationRepository
{
    Task AddAsync(JobApplication application);
}