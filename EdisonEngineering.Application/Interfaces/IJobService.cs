using EdisonEngineering.Application.DTOs;

public interface IJobService
{
    Task<List<JobDto>> GetJobsAsync();
    Task ApplyAsync(ApplyJobDto dto);
}