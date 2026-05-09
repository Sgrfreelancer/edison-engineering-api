using EdisonEngineering.Application.DTOs;
public interface ISolarCalculatorService
{
    Task<SolarCalculatorResponseDto> CalculateAsync(SolarCalculatorRequestDto request);
}