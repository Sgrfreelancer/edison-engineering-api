using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(
        EmailMessageDto email);
}
