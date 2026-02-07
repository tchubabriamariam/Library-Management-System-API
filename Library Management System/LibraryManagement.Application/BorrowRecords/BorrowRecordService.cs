using LibraryManagement.Application.BorrowRecords.DTOs;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrustructure.BorrowRecords;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.BorrowRecords
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly BorrowRecordRepository _borrowRepository;
        private readonly IBookService _bookService;
        private readonly ILogger<BorrowRecordService> _logger;

        public BorrowRecordService(
            BorrowRecordRepository borrowRepository, 
            IBookService bookService, 
            ILogger<BorrowRecordService> logger)
        {
            _borrowRepository = borrowRepository;
            _bookService = bookService;
            _logger = logger;
        }

        public async Task<PagedResult<BorrowRecordDto>> GetAllAsync(BorrowRecordFilterDto filter, CancellationToken token = default)
        {
            var query = _borrowRepository.Query();

            if (filter.PatronId.HasValue)
                query = query.Where(x => x.PatronId == filter.PatronId.Value);

            if (filter.BookId.HasValue)
                query = query.Where(x => x.BookId == filter.BookId.Value);

            var totalCount = query.Count();
            var items = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(br => MapToDto(br))
                .ToList();

            return new PagedResult<BorrowRecordDto>
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<BorrowRecordDto?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var record = await _borrowRepository.GetAsync(token, id);
            return record == null ? null : MapToDto(record);
        }

        public async Task<BorrowRecordDto> CheckOutBookAsync(CreateBorrowRecordDto createDto, CancellationToken token = default)
        {
            var availability = await _bookService.GetAvailabilityAsync(token, createDto.BookId);
            
            if (availability == null || !availability.IsAvailable)
            {
                throw new InvalidOperationException("Book is not available for borrowing");
            }

            var record = new BorrowRecord
            {
                BookId = createDto.BookId,
                PatronId = createDto.PatronId,
                BorrowDate = DateTime.UtcNow,
                //DueDate = DateTime.UtcNow.AddSeconds(14), // tested here if duedate worked
                DueDate = DateTime.UtcNow.AddDays(14), // 2-week
                Status = BorrowStatus.Borrowed 
            };

            await _borrowRepository.AddAsync(token, record);
            
            return MapToDto(record);
        }

        public async Task<bool> ReturnBookAsync(int id, CancellationToken token = default)
        {
            var record = await _borrowRepository.GetAsync(token, id);
            
            if (record == null || record.ReturnDate != null)
                return false;

            record.ReturnDate = DateTime.UtcNow;
            record.Status = BorrowStatus.Returned;

            await _borrowRepository.UpdateAsync(token, record);
            return true;
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetOverdueRecordsAsync(CancellationToken token = default)
        {
            var records = await _borrowRepository.GetOverdueAsync(token);
            return records.Select(MapToDto);
        }

        private static BorrowRecordDto MapToDto(BorrowRecord br) => new BorrowRecordDto
        {
            Id = br.Id,
            BookId = br.BookId,
            BookTitle = br.Book?.Title ?? "Unknown",
            PatronId = br.PatronId,
            PatronName = br.Patron != null ? $"{br.Patron.FirstName} {br.Patron.LastName}" : "Unknown",
            BorrowDate = br.BorrowDate,
            DueDate = br.DueDate,
            ReturnDate = br.ReturnDate,
            Status = br.Status
        };
    }
}