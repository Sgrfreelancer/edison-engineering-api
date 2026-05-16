using System.ComponentModel.DataAnnotations;

namespace EdisonEngineering.Application.DTOs;

public class SolarCalculatorRequestDto
{
    [Required]
    [StringLength(100)]
    public required string City { get; set; }

    [Range(1, 1000000)]
    public decimal MonthlyBill { get; set; }
}
