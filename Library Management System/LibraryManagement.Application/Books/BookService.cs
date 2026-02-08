using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Books;
using LibraryManagement.Infrustructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Books;

public class BookService : IBookService
{
    private readonly BookRepository _bookRepository;

    public BookService(BookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<PagedResult<BookDto>> GetAllAsync(
        CancellationToken token,
        int page = 1,
        int pageSize = 10)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _bookRepository
            .Query()
            .Include(b => b.Author)
            .Include(b => b.BorrowRecords);

        var totalCount = await query.CountAsync(token).ConfigureAwait(false);

        var books = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token)
            .ConfigureAwait(false);

        return new PagedResult<BookDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = books.Select(MapToDto).ToList()
        };
    }

    public async Task<BookDto?> GetByIdAsync(CancellationToken token, int id)
    {
        var book = await _bookRepository
            .Query()
            .Include(b => b.Author)
            .Include(b => b.BorrowRecords)
            .FirstOrDefaultAsync(b => b.Id == id, token);
        
        // tested exception handler
        //int y = 0;
        //int x = 10 / y;
        

        if (book == null)
        {
            throw new EntityNotFoundException("წიგნი", id);
        } 

        return MapToDto(book);
    }

    public async Task<PagedResult<BookDto>> SearchAsync(
        CancellationToken token,
        string? title,
        string? author,
        int page = 1,
        int pageSize = 10)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _bookRepository.SearchQuery(title, author);

        var totalCount = await query.CountAsync(token).ConfigureAwait(false);

        var books = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token)
            .ConfigureAwait(false);

        return new PagedResult<BookDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = books.Select(MapToDto).ToList()
        };
    }


    public async Task<int> CreateAsync(CancellationToken token, CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            ISBN = dto.ISBN,
            PublicationYear = dto.PublicationYear,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,
            Quantity = dto.Quantity,
            AuthorId = dto.AuthorId
        };

        await _bookRepository.AddAsync(token, book);
        return book.Id;
    }

    public async Task<bool> UpdateAsync(CancellationToken token, int id, UpdateBookDto dto)
    {
        var book = await _bookRepository.GetAsync(token, id);
        if (book == null)
            return false;

        book.Title = dto.Title;
        book.ISBN = dto.ISBN;
        book.PublicationYear = dto.PublicationYear;
        book.Description = dto.Description;
        book.CoverImageUrl = dto.CoverImageUrl;
        book.Quantity = dto.Quantity;
        book.AuthorId = dto.AuthorId;

        await _bookRepository.UpdateAsync(token, book);
        return true;
    }

    public async Task<bool> DeleteAsync(CancellationToken token, int id)
    {
        var exists = await _bookRepository.AnyAsync(token, b => b.Id == id);
        if (!exists)
            return false;

        await _bookRepository.RemoveAsync(token, id);
        return true;
    }

 
    // this is for map help 
    private static BookDto MapToDto(Book book)
    {
        var borrowedCount = book.BorrowRecords.Count(br => br.ReturnDate == null);

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.FirstName + " " + book.Author?.LastName,
            TotalCopies = book.Quantity,
            AvailableCopies = book.Quantity - borrowedCount
        };
    }
    
    
    
    public async Task<BookAvailabilityDto?> GetAvailabilityAsync(CancellationToken token, int id)
    {
        var book = await _bookRepository
            .Query()
            .Include(b => b.BorrowRecords)
            .FirstOrDefaultAsync(b => b.Id == id, token);

        if (book == null)
        {
            return null;
        }

        var borrowedCount = book.BorrowRecords.Count(br => br.ReturnDate == null);
        var availableCopies = Math.Max(
            book.Quantity - book.BorrowRecords.Count(br => br.ReturnDate == null),
            0
        );
       
        return new BookAvailabilityDto
        {
            BookId = book.Id,
            AvailableCopies = availableCopies,
            IsAvailable = availableCopies > 0
        };
    }

}
