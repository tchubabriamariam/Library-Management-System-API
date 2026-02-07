using LibraryManagement.Application.Books.DTOs;

namespace LibraryManagement.Application.Patrons.DTOs;

public class PatronWithBooksDto : PatronDto
{
    public ICollection<BookDto> BorrowedBooks { get; set; } = new List<BookDto>();
}