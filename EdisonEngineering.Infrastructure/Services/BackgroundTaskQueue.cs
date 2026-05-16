using System.Threading.Channels;

using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;

namespace EdisonEngineering.Infrastructure.Services;

public class BackgroundTaskQueue
    : IBackgroundTaskQueue
{
    private readonly Channel<EmailMessageDto>
        _queue;

    public BackgroundTaskQueue()
    {
        _queue =
            Channel.CreateUnbounded
                <EmailMessageDto>();
    }

    public void QueueEmail(
        EmailMessageDto email)
    {
        _queue.Writer.TryWrite(email);
    }

    public async Task<EmailMessageDto>
        DequeueAsync(
            CancellationToken cancellationToken)
    {
        return await _queue.Reader
            .ReadAsync(cancellationToken);
    }
}
