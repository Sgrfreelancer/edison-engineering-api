using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.Application.Interfaces;

public interface IBackgroundTaskQueue
{
    void QueueEmail(
        EmailMessageDto email);

    Task<EmailMessageDto>
        DequeueAsync(
            CancellationToken cancellationToken);
}
