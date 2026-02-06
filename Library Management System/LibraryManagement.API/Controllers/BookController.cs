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
    [HttpGet] // needs more testing
    public async Task<ActionResult<PagedResult<BookDto>>> GetAll(
        CancellationToken token,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _bookService.GetAllAsync(token, page, pageSize);
        return Ok(result);
    }
    
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


    [HttpPost] // i have one author with id 1 and use that, works
    public async Task<ActionResult<int>> Create(CancellationToken token, [FromBody] CreateBookDto dto)
    {
        var id = await _bookService.CreateAsync(token, dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

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