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

    [HttpGet]
    public async Task<ActionResult<PagedResult<PatronDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken token = default)
    {
        var result = await _patronService.GetAllPatronsAsync(page, pageSize, token);
        return Ok(result);
    }

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

    [HttpGet("{id}/books")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBorrowedBooks(int id, CancellationToken token = default)
    {
        var books = await _patronService.GetBorrowedBooksAsync(id, token);
        return Ok(books);
    }

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
