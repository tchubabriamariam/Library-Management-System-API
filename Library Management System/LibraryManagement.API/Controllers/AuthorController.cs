using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Authors.DTOs;
using LibraryManagement.Application.Books.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Retrieves a paginated list of all authors.
    /// </summary>
    /// <param name="page">The page number to retrieve (starts at 1).</param>
    /// <param name="pageSize">The number of books per page.</param>
    /// <response code="200">Returns the list of authors.</response>
    [ProducesResponseType(typeof(PagedResult<AuthorDto>), StatusCodes.Status200OK)]
    [HttpGet] // works
    public async Task<ActionResult<PagedResult<AuthorDto>>> GetAll(
        CancellationToken token,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _authorService.GetAllAsync(token, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves details for a specific author by their ID.
    /// </summary>
    /// <param name="id">The author's unique identifier.</param>
    /// <response code="200">Returns the requested author details.</response>
    /// <response code="404">If the author is not found in the system.</response>
    [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")] // works
    public async Task<ActionResult<AuthorDto>> GetById(CancellationToken token, int id)
    {
        var author = await _authorService.GetByIdAsync(token, id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    /// <summary>
    /// Retrieves all books written by a specific author.
    /// </summary>
    /// <param name="id">Unique ID of the author</param>
    /// <param name="page">Page number, starts at 1</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <response code="200">Returns a paginated list of books for the author.</response>
    [ProducesResponseType(typeof(PagedResult<BookDto>), StatusCodes.Status200OK)]
    [HttpGet("{id:int}/books")] // works
    public async Task<ActionResult<PagedResult<BookDto>>> GetBooksByAuthor(
        CancellationToken token,
        int id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _authorService.GetBooksByAuthorAsync(token, id, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Registers a new author in the library system.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/Authors
    ///     {
    ///        "firstName": "Akaki",
    ///        "lastName": "Tsereteli",
    ///        "biography": "Famous Georgian poet and public figure.",
    ///     }
    /// </remarks>
    /// <response code="201">Returns the ID of the newly created author.</response>
    /// <response code="400">If the input data is invalid.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost] // works
    public async Task<ActionResult<int>> Create(
        CancellationToken token,
        [FromBody] CreateAuthorDto dto)
    {
        var id = await _authorService.CreateAsync(token, dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates the details of an existing author.
    /// </summary>
    /// <param name="id">The unique ID of the book to update</param>
    /// <response code="204">Update successful.</response>
    /// <response code="404">Author not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")] // works
    public async Task<IActionResult> Update(
        CancellationToken token,
        int id,
        [FromBody] UpdateAuthorDto dto)
    {
        var updated = await _authorService.UpdateAsync(token, id, dto);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Deletes an author from the system.
    /// </summary>
    /// /// <param name="id">Unique ID of the author</param>
    /// <response code="204">Deletion successful.</response>
    /// <response code="404">Author not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")] // works
    public async Task<IActionResult> Delete(CancellationToken token, int id)
    {
        var deleted = await _authorService.DeleteAsync(token, id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
