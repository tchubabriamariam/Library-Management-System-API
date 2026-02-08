using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Patrons;
using LibraryManagement.Application.Patrons.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers;

[ApiController]
[Route("api/patrons")]
public class PatronsController : ControllerBase
{
    private readonly IPatronService _patronService;

    public PatronsController(IPatronService patronService)
    {
        _patronService = patronService;
    }

    /// <summary>
    /// Retrieves a paginated list of all library patrons.
    /// </summary>
    /// <param name="page">The page number to retrieve (starts at 1).</param>
    /// <param name="pageSize">The number of patrons per page.</param>
    /// <response code="200">Returns the paginated list of patrons.</response>
    [ProducesResponseType(typeof(PagedResult<PatronDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<PagedResult<PatronDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken token = default)
    {
        var result = await _patronService.GetAllPatronsAsync(page, pageSize, token);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves details for a specific patron by their unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the patron.</param>
    /// <response code="200">Returns the requested patron details.</response>
    /// <response code="404">If the patron is not found.</response>
    [ProducesResponseType(typeof(PatronDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<PatronDto>> GetById(int id, CancellationToken token = default)
    {
        var patron = await _patronService.GetPatronByIdAsync(id, token);

        if (patron == null)
        {
            return NotFound();
        }

        return Ok(patron);
    }

    /// <summary>
    /// Retrieves a list of books currently borrowed by a specific patron.
    /// </summary>
    /// <param name="id">The unique identifier of the patron.</param>
    /// <response code="200">Returns a collection of books borrowed by the patron.</response>
    /// <response code="404">If the patron is not found.</response>
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id}/books")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBorrowedBooks(int id, CancellationToken token = default)
    {
        var books = await _patronService.GetBorrowedBooksAsync(id, token);
        return Ok(books);
    }

    /// <summary>
    /// Registers a new patron in the library system.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/patrons
    ///     {
    ///        "firstName": "Mariam",
    ///        "lastName": "Tchubabria",
    ///        "email": "mariam@example.com"
    ///     }
    /// </remarks>
    /// <param name="createDto">The patron creation data.</param>
    /// <response code="201">Returns the newly created patron.</response>
    /// <response code="400">If the input data is invalid.</response>
    [ProducesResponseType(typeof(PatronDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<PatronDto>> Create(
        [FromBody] CreatePatronDto createDto,
        CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPatron = await _patronService.CreatePatronAsync(createDto, token);

        return CreatedAtAction(nameof(GetById), new { id = createdPatron.Id }, createdPatron);
    }

    /// <summary>
    /// Updates an existing patron's information.
    /// </summary>
    /// <param name="id">The unique ID of the patron to update.</param>
    /// <response code="204">Update successful.</response>
    /// <response code="404">Patron not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePatronDto updateDto,
        CancellationToken token = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _patronService.UpdatePatronAsync(id, updateDto, token);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a patron from the system.
    /// </summary>
    /// <param name="id">The unique ID of the patron to delete.</param>
    /// <response code="204">Deletion successful.</response>
    /// <response code="404">Patron not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        var success = await _patronService.DeletePatronAsync(id, token);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
