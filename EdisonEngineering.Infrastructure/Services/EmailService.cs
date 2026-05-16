using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;

using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Infrastructure.Services;

public class EmailService
    : IEmailService
{
    private readonly ILogger<EmailService>
        _logger;

    public EmailService(
        ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(
        EmailMessageDto email)
    {
        _logger.LogInformation(
            """
            Sending Email
            To: {To}
            Subject: {Subject}
            Body: {Body}
            """,

            email.To,
            email.Subject,
            email.Body);

        await Task.Delay(1000);
    }
}
