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

        
        [HttpGet] // works
        public async Task<ActionResult<PagedResult<AuthorDto>>> GetAll(
            CancellationToken token,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _authorService.GetAllAsync(token, page, pageSize);
            return Ok(result);
        }

        
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

        
        [HttpGet("{id:int}/books")] //works
        public async Task<ActionResult<PagedResult<BookDto>>> GetBooksByAuthor(
            CancellationToken token,
            int id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _authorService.GetBooksByAuthorAsync(token, id, page, pageSize);

            return Ok(result);

        }

        
        [HttpPost] // works
        public async Task<ActionResult<int>> Create(CancellationToken token, [FromBody] CreateAuthorDto dto)
        {
            var id = await _authorService.CreateAsync(token, dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        
        [HttpPut("{id:int}")] // works
        public async Task<IActionResult> Update(CancellationToken token, int id, [FromBody] UpdateAuthorDto dto)
        {
            var updated = await _authorService.UpdateAsync(token, id, dto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

    
        [HttpDelete("{id:int}")] //works
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