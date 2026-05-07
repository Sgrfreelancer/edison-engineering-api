public interface ISolarCalculatorService
{
    Task<SolarCalculatorResponseDto> CalculateAsync(SolarCalculatorRequestDto request);
}