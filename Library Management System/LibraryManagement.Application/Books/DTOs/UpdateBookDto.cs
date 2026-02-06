namespace LibraryManagement.Application.Books.DTOs;

public class UpdateBookDto
{
    public string Title { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int AuthorId { get; set; }
    public int TotalCopies { get; set; }
    public  int PublicationYear { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int Quantity { get; set; }
}