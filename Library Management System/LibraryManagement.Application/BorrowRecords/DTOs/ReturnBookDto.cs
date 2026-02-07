namespace LibraryManagement.Application.BorrowRecords.DTOs;

public class ReturnBookDto
{
    public DateTime ReturnDate { get; set; } = DateTime.UtcNow;
}