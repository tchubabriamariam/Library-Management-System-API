namespace LibraryManagement.Application.Books;

using LibraryManagement.Application.Books.DTOs;


public interface IBookService
{
    Task<PagedResult<BookDto>> GetAllAsync(CancellationToken token, int page = 1, int pageSize = 10);

    Task<BookDto?> GetByIdAsync(CancellationToken token, int id);

    Task<PagedResult<BookDto>> SearchAsync(
        CancellationToken token,
        string? title,
        string? author,
        int page = 1,
        int pageSize = 10);

    Task<int> CreateAsync(CancellationToken token, CreateBookDto dto);

    Task<bool> UpdateAsync(CancellationToken token, int id, UpdateBookDto dto);

    Task<bool> DeleteAsync(CancellationToken token, int id);
    Task<BookAvailabilityDto?> GetAvailabilityAsync(CancellationToken token, int id);

}
