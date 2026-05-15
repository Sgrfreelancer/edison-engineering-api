using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto dto);

    Task<LoginResponseDto?> RefreshTokenAsync(
        RefreshTokenRequestDto dto);
}