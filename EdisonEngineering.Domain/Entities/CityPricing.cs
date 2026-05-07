namespace EdisonEngineering.Domain.Entities;

public class CityPricing
{
    public int Id { get; set; }

    public string City { get; set; }

    public decimal RatePerUnit { get; set; }
    public decimal CostPerKW { get; set; }
}