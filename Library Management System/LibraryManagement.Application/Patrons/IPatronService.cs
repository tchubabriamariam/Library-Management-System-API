using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Patrons.DTOs;

namespace LibraryManagement.Application.Patrons;

public interface IPatronService
{
    Task<PagedResult<PatronDto>> GetAllPatronsAsync(int pageNumber, int pageSize, CancellationToken token = default);
    Task<PatronDto?> GetPatronByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<BookDto>> GetBorrowedBooksAsync(int patronId, CancellationToken token = default);
    Task<PatronDto> CreatePatronAsync(CreatePatronDto createDto, CancellationToken token = default);
    Task<bool> UpdatePatronAsync(int id, UpdatePatronDto updateDto, CancellationToken token = default);
    Task<bool> DeletePatronAsync(int id, CancellationToken token = default);
}