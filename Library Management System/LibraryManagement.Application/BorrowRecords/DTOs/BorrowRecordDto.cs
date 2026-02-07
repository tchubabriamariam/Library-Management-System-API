using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.BorrowRecords.DTOs;

public class BorrowRecordDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public int PatronId { get; set; }
    public string PatronName { get; set; } = null!;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }
}