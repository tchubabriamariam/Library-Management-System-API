namespace LibraryManagement.Application.Books.DTOs;

public class BookDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ISBN { get; set; } = null!;

    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }
}