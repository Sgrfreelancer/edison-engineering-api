using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Interfaces;

public interface ICityPricingRepository
{
    Task<CityPricing?> GetByCityAsync(string city);
}