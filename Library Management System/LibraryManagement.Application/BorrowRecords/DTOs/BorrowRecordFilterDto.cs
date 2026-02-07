namespace LibraryManagement.Application.BorrowRecords.DTOs;

public class BorrowRecordFilterDto
{
    public int? PatronId { get; set; }
    public int? BookId { get; set; }
    public bool? IsOverdue { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}