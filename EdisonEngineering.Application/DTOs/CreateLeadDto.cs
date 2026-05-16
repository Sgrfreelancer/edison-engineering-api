using System.ComponentModel.DataAnnotations;

namespace EdisonEngineering.Application.DTOs;

public class CreateLeadDto
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [Phone]
    [StringLength(15)]
    public required string Phone { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    [Required]
    [StringLength(100)]
    public required string City { get; set; }

    [Required]
    [StringLength(100)]
    public required string ServiceType { get; set; }

    [StringLength(1000)]
    public string? Message { get; set; }

    [StringLength(100)]
    public string? Source { get; set; }
}
