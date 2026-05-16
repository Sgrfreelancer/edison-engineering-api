public class SolarCalculatorResponseDto
{
    public decimal EstimatedUnits { get; set; }
    public decimal SystemSizeKW { get; set; }

    public decimal InstallationCost { get; set; }

    public decimal SubsidyAmount { get; set; }
    public decimal FinalCost { get; set; }

    public decimal MonthlySavings { get; set; }
    public decimal YearlySavings { get; set; }

    public decimal PaybackYears { get; set; }
}