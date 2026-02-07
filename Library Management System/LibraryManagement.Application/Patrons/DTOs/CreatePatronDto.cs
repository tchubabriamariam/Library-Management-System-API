using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Patrons.DTOs;

public class CreatePatronDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}