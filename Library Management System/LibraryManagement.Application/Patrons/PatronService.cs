using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Patrons.DTOs;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Patrons; 
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Patrons;

public class PatronService : IPatronService
{
    // i want to inject here IPatronRepository but cant since it doesnt have baserepository features which i need
    private readonly PatronRepository _patronRepository;
    private readonly ILogger<PatronService> _logger;

    public PatronService(PatronRepository patronRepository, ILogger<PatronService> logger)
    {
        _patronRepository = patronRepository;
        _logger = logger;
    }

    public async Task<PagedResult<PatronDto>> GetAllPatronsAsync(int pageNumber, int pageSize, CancellationToken token = default)
    {
        _logger.LogInformation("Getting all patrons");
        var query = _patronRepository.Query();
        
        var totalCount = query.Count();
        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => MapToDto(p))
            .ToList();

        return new PagedResult<PatronDto>
        {
            Page = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<PatronDto?> GetPatronByIdAsync(int id, CancellationToken token = default)
    {
        _logger.LogInformation("Getting patron by id");
        var patron = await _patronRepository.GetAsync(token, id);
        return patron == null ? null : MapToDto(patron);
    }

    public async Task<IEnumerable<BookDto>> GetBorrowedBooksAsync(int patronId, CancellationToken token = default)
    {
        _logger.LogInformation("Getting borrowed books");
        var patronWithRecords = await _patronRepository.GetWithBorrowRecordsAsync(token, patronId);
    
        if (patronWithRecords == null)
        {
            return Enumerable.Empty<BookDto>();
        }

        return patronWithRecords.BorrowRecords
            .Where(br => br.ReturnDate == null) 
            .Select(br => new BookDto
            {
                Id = br.Book.Id,
                Title = br.Book.Title,
                ISBN = br.Book.ISBN
            });
    }

    public async Task<PatronDto> CreatePatronAsync(CreatePatronDto createDto, CancellationToken token = default)
    {
        _logger.LogInformation("Creating patron");
        var patron = new Patron
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            MembershipDate = DateTime.UtcNow 
        };

        await _patronRepository.AddAsync(token, patron);
    
        return MapToDto(patron);
    }
    public async Task<bool> UpdatePatronAsync(int id, UpdatePatronDto updateDto, CancellationToken token = default)
    {
        _logger.LogInformation("Updating patron");
        var existingPatron = await _patronRepository.GetAsync(token, id);
        if (existingPatron == null) return false;

        existingPatron.FirstName = updateDto.FirstName;
        existingPatron.LastName = updateDto.LastName;
        existingPatron.Email = updateDto.Email;
        await _patronRepository.UpdateAsync(token, existingPatron);
        return true;
    }

    public async Task<bool> DeletePatronAsync(int id, CancellationToken token = default)
    {
        _logger.LogInformation("Deleting patron");
        var existingPatron = await _patronRepository.GetAsync(token, id);
        if (existingPatron == null) return false;

        await _patronRepository.RemoveAsync(token, existingPatron);
        return true;
    }

    private static PatronDto MapToDto(Patron patron) => new PatronDto
    {
        Id = patron.Id,
        FirstName = patron.FirstName,
        LastName = patron.LastName,
        Email = patron.Email,
        MembershipDate = patron.MembershipDate
    };
}

