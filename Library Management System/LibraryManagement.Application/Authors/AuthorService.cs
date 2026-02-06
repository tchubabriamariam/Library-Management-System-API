using LibraryManagement.Application.Authors.DTOs;
using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Authors;

public class AuthorService : IAuthorService
{
    private readonly BaseRepository<Author> _authorRepository;

    public AuthorService(BaseRepository<Author> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<PagedResult<AuthorDto>> GetAllAsync(
        CancellationToken token,
        int page = 1,
        int pageSize = 10)
    {
        var query = _authorRepository.Query();

        var totalCount = await query.CountAsync(token);

        var authors = await query
            .OrderBy(a => a.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PagedResult<AuthorDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = authors.Select(MapToDto).ToList()
        };
    }

    public async Task<AuthorDto?> GetByIdAsync(CancellationToken token, int id)
    {
        var author = await _authorRepository.GetAsync(token, id);
        return author == null ? null : MapToDto(author);
    }

    public async Task<PagedResult<AuthorDto>> SearchAsync(
        CancellationToken token,
        string query,
        int page = 1,
        int pageSize = 10)
    {
        var authorsQuery = _authorRepository
            .Query()
            .Where(a =>
                a.FirstName.Contains(query) ||
                a.LastName.Contains(query));

        var totalCount = await authorsQuery.CountAsync(token);

        var authors = await authorsQuery
            .OrderBy(a => a.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PagedResult<AuthorDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = authors.Select(MapToDto).ToList()
        };
    }

    public async Task<int> CreateAsync(CancellationToken token, CreateAuthorDto dto)
    {
        var author = new Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Biography = dto.Biography
        };

        await _authorRepository.AddAsync(token, author);
        return author.Id;
    }

    public async Task<bool> UpdateAsync(CancellationToken token, int id, UpdateAuthorDto dto)
    {
        var author = await _authorRepository.GetAsync(token, id);
        if (author == null) return false;

        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        author.Biography = dto.Biography;

        await _authorRepository.UpdateAsync(token, author);
        return true;
    }

    public async Task<bool> DeleteAsync(CancellationToken token, int id)
    {
        var exists = await _authorRepository.AnyAsync(token, a => a.Id == id);
        if (!exists) return false;

        await _authorRepository.RemoveAsync(token, id);
        return true;
    }

    private static AuthorDto MapToDto(Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Biography = author.Biography
        };
    }
}