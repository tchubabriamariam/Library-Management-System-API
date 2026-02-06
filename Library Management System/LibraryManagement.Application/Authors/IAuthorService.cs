using LibraryManagement.Application.Authors.DTOs;
using LibraryManagement.Application.Books.DTOs;

namespace LibraryManagement.Application.Authors;

public interface IAuthorService
{
    Task<PagedResult<AuthorDto>> GetAllAsync(CancellationToken token, int page = 1, int pageSize = 10);

    Task<AuthorDto?> GetByIdAsync(CancellationToken token, int id);

    Task<PagedResult<AuthorDto>> SearchAsync(CancellationToken token, string query, int page = 1, int pageSize = 10);

    Task<int> CreateAsync(CancellationToken token, CreateAuthorDto dto);

    Task<bool> UpdateAsync(CancellationToken token, int id, UpdateAuthorDto dto);

    Task<bool> DeleteAsync(CancellationToken token, int id);
}