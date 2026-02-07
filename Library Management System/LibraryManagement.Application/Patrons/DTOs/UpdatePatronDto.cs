using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Patrons.DTOs;

public class UpdatePatronDto
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}