using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.BorrowRecords.DTOs;

namespace LibraryManagement.Application.BorrowRecords;

public interface IBorrowRecordService
{
    Task<PagedResult<BorrowRecordDto>> GetAllAsync(
        BorrowRecordFilterDto filter, 
        CancellationToken token = default);
    Task<BorrowRecordDto?> GetByIdAsync(int id, CancellationToken token = default);
    Task<BorrowRecordDto> CheckOutBookAsync(
        CreateBorrowRecordDto createDto, 
        CancellationToken token = default);
    Task<bool> ReturnBookAsync(int id, CancellationToken token = default);
    Task<IEnumerable<BorrowRecordDto>> GetOverdueRecordsAsync(CancellationToken token = default);
}