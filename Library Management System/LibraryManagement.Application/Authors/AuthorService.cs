using Azure;
using LibraryManagement.Application.Authors.DTOs;
using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Authors;

public class AuthorService : IAuthorService
{
    private readonly BaseRepository<Author> _authorRepository;
    private readonly BaseRepository<Book> _bookRepository;


    public AuthorService(BaseRepository<Author> authorRepository, BaseRepository<Book> bookRepository)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        
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
    
    public async Task<PagedResult<BookDto>> GetBooksByAuthorAsync(
        CancellationToken token,
        int authorId,
        int page,
        int pageSize)
    {
        var query = _bookRepository
            .Query()
            .Where(b => b.AuthorId == authorId)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                AuthorId = b.AuthorId,
                AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                TotalCopies = b.Quantity,
                AvailableCopies = Math.Max(
                    0,
                    b.Quantity - b.BorrowRecords.Count(br => br.ReturnDate == null)
                )
            });

        var totalCount = await query.CountAsync(token);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PagedResult<BookDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }



    private static BookDto BookServiceMapToDto(Book book)
    {
        var borrowedCount = book.BorrowRecords.Count(br => br.ReturnDate == null);

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            AuthorId = book.AuthorId,
            AuthorName = (book.Author != null)
                ? $"{book.Author.FirstName} {book.Author.LastName}"
                : null,
            TotalCopies = book.Quantity,
            AvailableCopies = book.Quantity - borrowedCount
        };
    }
}