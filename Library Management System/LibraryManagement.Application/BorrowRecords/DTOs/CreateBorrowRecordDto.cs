using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.BorrowRecords.DTOs;

public class CreateBorrowRecordDto
{
    [Required]
    public int BookId { get; set; }
    [Required]
    public int PatronId { get; set; }
}