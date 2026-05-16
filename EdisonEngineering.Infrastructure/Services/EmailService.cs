using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.Common.Settings;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

namespace EdisonEngineering.Infrastructure.Services;

public class EmailService
    : IEmailService
{
    private readonly EmailSettings
        _settings;

    private readonly ILogger<EmailService>
        _logger;

    public EmailService(
        IOptions<EmailSettings> settings,

        ILogger<EmailService> logger)
    {
        _settings = settings.Value;

        _logger = logger;
    }

    public async Task SendAsync(
        EmailMessageDto email)
    {
        try
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.SenderName,
                    _settings.SenderEmail));

            message.To.Add(
                MailboxAddress.Parse(
                    email.To));

            message.Subject =
                email.Subject;

            message.Body =
                new TextPart("html")
                {
                    Text = email.Body
                };

            using var client =
                new SmtpClient();

            await client.ConnectAsync(
                _settings.SmtpServer,

                _settings.Port,

                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _settings.Username,

                _settings.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);

            _logger.LogInformation(
                "Email sent successfully to: {To}",
                email.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send email to: {To}",
                email.To);

            throw;
        }
    }
}
