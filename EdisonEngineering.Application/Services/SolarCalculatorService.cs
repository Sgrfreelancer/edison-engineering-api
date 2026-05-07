using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class SolarCalculatorService : ISolarCalculatorService
{
    private readonly IConfigRepository _configRepo;
    private readonly ICityPricingRepository _cityPricingRepo;
    private readonly ISubsidyRepository _subsidyRepo;
    private readonly ISlabRepository _slabRepo;
    private readonly ILogger<SolarCalculatorService> _logger;

    public SolarCalculatorService(
        IConfigRepository configRepo,
        ICityPricingRepository cityPricingRepo,
        ISubsidyRepository subsidyRepo,
        ISlabRepository slabRepo,
        ILogger<SolarCalculatorService> logger)
    {
        _configRepo = configRepo;
        _cityPricingRepo = cityPricingRepo;
        _subsidyRepo = subsidyRepo;
        _slabRepo = slabRepo;
        _logger = logger;
    }

    private decimal CalculateUnitsFromSlab(
        decimal bill,
        List<ElectricitySlab> slabs)
    {
        decimal units = 0;
        decimal remaining = bill;

        foreach (var slab in slabs)
        {
            var slabUnits = slab.MaxUnit - slab.MinUnit + 1;
            var slabCost = slabUnits * slab.Rate;

            if (remaining > slabCost)
            {
                units += slabUnits;
                remaining -= slabCost;
            }
            else
            {
                units += remaining / slab.Rate;
                break;
            }
        }

        return units;
    }

    public async Task<SolarCalculatorResponseDto> CalculateAsync(
        SolarCalculatorRequestDto request)
    {
        _logger.LogInformation(
            "Solar calculation started for city: {City}, MonthlyBill: {MonthlyBill}",
            request.City,
            request.MonthlyBill);

        // STEP 1: Get Config Values
        var config = await _configRepo.GetAllAsync();

        var unitsPerKW = decimal.Parse(config["UnitsPerKW"]);
        var defaultRatePerUnit = decimal.Parse(config["RatePerUnit"]);
        var defaultCostPerKW = decimal.Parse(config["CostPerKW"]);

        _logger.LogInformation(
            "Configuration values loaded successfully");

        // STEP 2: Get City Pricing
        var cityPricing = await _cityPricingRepo.GetByCityAsync(request.City);

        var ratePerUnit = cityPricing?.RatePerUnit
            ?? defaultRatePerUnit;

        var costPerKW = cityPricing?.CostPerKW
            ?? defaultCostPerKW;

        _logger.LogInformation(
            "Pricing resolved for city: {City}",
            request.City);

        // STEP 3: Calculate Units
        var slabs = await _slabRepo.GetAllAsync();

        decimal units;

        if (slabs != null && slabs.Any())
        {
            _logger.LogInformation(
                "Using slab-based calculation");

            units = CalculateUnitsFromSlab(
                request.MonthlyBill,
                slabs);
        }
        else
        {
            _logger.LogWarning(
                "No slabs configured. Using default rate calculation");

            units = request.MonthlyBill / ratePerUnit;
        }

        // STEP 4: Calculate System Size
        var systemSize = units / unitsPerKW;

        // STEP 5: Calculate Installation Cost
        var cost = systemSize * costPerKW;

        _logger.LogInformation(
            "Base solar cost calculated successfully");

        // STEP 6: Apply Subsidy
        var subsidy = await _subsidyRepo.GetByKWAsync(systemSize);

        var subsidyAmount = subsidy != null
            ? systemSize * subsidy.SubsidyAmountPerKW
            : 0;

        var finalCost = cost - subsidyAmount;

        _logger.LogInformation(
            "Subsidy applied. SubsidyAmount: {SubsidyAmount}, FinalCost: {FinalCost}",
            subsidyAmount,
            finalCost);

        // STEP 7: Savings
        var yearlySavings = request.MonthlyBill * 12;

        var payback = yearlySavings > 0
            ? finalCost / yearlySavings
            : 0;

        _logger.LogInformation(
            "Savings and payback calculated successfully");

        // STEP 8: Response
        var response = new SolarCalculatorResponseDto
        {
            EstimatedUnits = Math.Round(units, 2),

            SystemSizeKW = Math.Round(systemSize, 2),

            InstallationCost = Math.Round(cost, 0),

            SubsidyAmount = Math.Round(subsidyAmount, 0),

            FinalCost = Math.Round(finalCost, 0),

            MonthlySavings = request.MonthlyBill,

            YearlySavings = yearlySavings,

            PaybackYears = Math.Round(payback, 1)
        };

        _logger.LogInformation(
            "Solar calculation completed successfully for city: {City}",
            request.City);

        return response;
    }
}