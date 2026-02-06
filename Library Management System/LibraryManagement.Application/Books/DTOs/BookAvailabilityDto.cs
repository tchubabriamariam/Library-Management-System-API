namespace LibraryManagement.Application.Books.DTOs;

public class BookAvailabilityDto
{
    public int BookId { get; set; }
    public bool IsAvailable { get; set; }
    public int AvailableCopies { get; set; }
}
