using EdisonEngineering.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.API.BackgroundServices;

public class EmailBackgroundService
    : BackgroundService
{
    private readonly IServiceProvider
        _serviceProvider;

    private readonly ILogger
        <EmailBackgroundService>
        _logger;

    public EmailBackgroundService(
        IServiceProvider serviceProvider,

        ILogger<EmailBackgroundService>
            logger)
    {
        _serviceProvider = serviceProvider;

        _logger = logger;
    }

    protected override async Task
        ExecuteAsync(
            CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Email background service started");

        using var scope =
            _serviceProvider
                .CreateScope();

        var queue =
            scope.ServiceProvider
                .GetRequiredService
                    <IBackgroundTaskQueue>();

        var emailService =
            scope.ServiceProvider
                .GetRequiredService
                    <IEmailService>();

        while (!stoppingToken
            .IsCancellationRequested)
        {
            try
            {
                var email =
                    await queue
                        .DequeueAsync(
                            stoppingToken);

                await emailService
                    .SendAsync(email);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation(
                    "Email background service shutdown requested");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while processing email queue");
            }
        }
    }
}
