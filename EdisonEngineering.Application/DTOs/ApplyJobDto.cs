using System.ComponentModel.DataAnnotations;

namespace EdisonEngineering.Application.DTOs;

public class ApplyJobDto
{
    [Required]
    public int JobId { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public required string Email { get; set; }

    [Required]
    [Phone]
    [StringLength(15)]
    public required string Mobile { get; set; }

    [Required]
    [Url]
    [StringLength(500)]
    public required string ResumeUrl { get; set; }
}
