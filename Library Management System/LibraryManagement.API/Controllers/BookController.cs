using LibraryManagement.Application.Books;
using LibraryManagement.Application.Books.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }
    /// <summary>
    /// Retrieves a paginated list of all books in the library.
    /// </summary>
    /// <param name="page">The page number to retrieve (defaults to 1).</param>
    /// <param name="pageSize">The number of items per page (defaults to 10).</param>
    /// <response code="200">Returns the paginated list of books.</response>
    [ProducesResponseType(typeof(PagedResult<BookDto>), StatusCodes.Status200OK)]
    [HttpGet] // needs more testing
    public async Task<ActionResult<PagedResult<BookDto>>> GetAll(
        CancellationToken token,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _bookService.GetAllAsync(token, page, pageSize);
        return Ok(result);
    }
    /// <summary>
    /// Retrieves a specific book by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the book.</param>
    /// <response code="200">Returns the requested book details.</response>
    /// <response code="404">If the book was not found.</response>
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")] // works
    public async Task<ActionResult<BookDto>> GetById(CancellationToken token, int id)
    {
        var book = await _bookService.GetByIdAsync(token, id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    /// <summary>
    /// Searches for books based on title or author keywords.
    /// </summary>
    /// <param name="title">Keyword to search for in book titles.</param>
    /// <param name="author">Keyword to search for in author names.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <response code="200">Returns search results matching the criteria.</response>
    /// <response code="400">If both title and author parameters are empty.</response>
    [ProducesResponseType(typeof(PagedResult<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<BookDto>>> Search(
        CancellationToken token,
        [FromQuery] string? title,
        [FromQuery] string? author,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
        {
            return BadRequest("At least one search parameter title or author is required");
        }

        var result = await _bookService.SearchAsync(token, title, author, page, pageSize);
        return Ok(result);
    }


    /// <summary>
    /// Registers a new book in the library system.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/Books
    ///     {
    ///        "title": "1984",
    ///        "isbn": "978-0451524935",
    ///        "publicationYear": 1949,
    ///        "authorId": 1,
    ///        "quantity": 5,
    ///        "description": "A classic dystopian novel about totalitarianism.",
    ///        "coverImageUrl": "https://example.com/1984.jpg"
    ///     }
    /// </remarks>
    /// <param name="dto">The book data transfer object.</param>
    /// <response code="201">Returns the ID of the newly created book.</response>
    /// <response code="400">If the validation fails.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost] // i have one author with id 1 and use that, works
    public async Task<ActionResult<int>> Create(CancellationToken token, [FromBody] CreateBookDto dto)
    {
        var id = await _bookService.CreateAsync(token, dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing book's details.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <response code="204">Update successful.</response>
    /// <response code="404">Book not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")] //works
    public async Task<IActionResult> Update(CancellationToken token, int id, [FromBody] UpdateBookDto dto)
    {
        var ok = await _bookService.UpdateAsync(token, id, dto);
        if (!ok)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Removes a book from the library system.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <response code="204">Deletion successful.</response>
    /// <response code="404">Book not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")] // works
    public async Task<IActionResult> Delete(CancellationToken token, int id)
    {
        var ok = await _bookService.DeleteAsync(token, id);
        if (!ok)
        {
            return NotFound();
        }
        return NoContent();
    }
    
    
    /// <summary>
    /// Checks the real-time availability of a book (total vs. borrowed copies).
    /// </summary>
    /// <param name="id">The unique ID of the book.</param>
    /// <response code="200">Returns current availability data.</response>
    /// <response code="404">If the book does not exist.</response>
    [ProducesResponseType(typeof(BookAvailabilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}/availability")]
    public async Task<ActionResult<BookAvailabilityDto>> GetAvailability(
        CancellationToken token,
        int id)
    {
        var availability = await _bookService.GetAvailabilityAsync(token, id);
        if (availability == null)
        {
            return NotFound();
        }

        return Ok(availability);
    }

}