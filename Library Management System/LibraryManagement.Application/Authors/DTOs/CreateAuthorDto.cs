namespace LibraryManagement.Application.Authors.DTOs;

public class CreateAuthorDto
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Biography { get; set; }
}