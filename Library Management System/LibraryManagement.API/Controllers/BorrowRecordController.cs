using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.BorrowRecords;
using LibraryManagement.Application.BorrowRecords.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/borrow-records")]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordService _borrowRecordService;

        public BorrowRecordsController(IBorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }

        /// <summary>
        /// Retrieves a paginated list of borrowing records with optional filtering.
        /// </summary>
        /// <response code="200">Returns the filtered list of borrow records.</response>
        [ProducesResponseType(typeof(PagedResult<BorrowRecordDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<PagedResult<BorrowRecordDto>>> GetAll([FromQuery] BorrowRecordFilterDto filter, CancellationToken token = default)
        {
            var result = await _borrowRecordService.GetAllAsync(filter, token);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific borrow record by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the borrow record.</param>
        /// <response code="200">Returns the requested borrow record.</response>
        /// <response code="404">If the record does not exist.</response>
        [ProducesResponseType(typeof(BorrowRecordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecordDto>> GetById(int id, CancellationToken token = default)
        {
            var record = await _borrowRecordService.GetByIdAsync(id, token);
            
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        /// <summary>
        /// Processes a new book check-out for a patron.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/borrow-records
        ///     {
        ///        "bookId": 1,
        ///        "patronId": 12,
        ///     }
        /// </remarks>
        /// <param name="createDto">Details of the book and patron for the check-out.</param>
        /// <response code="201">Returns the newly created borrow record.</response>
        /// <response code="400">If the book is out of stock or the patron already has this book.</response>
        [ProducesResponseType(typeof(BorrowRecordDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<BorrowRecordDto>> CheckOut([FromBody] CreateBorrowRecordDto createDto, CancellationToken token = default)
        {
            try
            {
                var record = await _borrowRecordService.CheckOutBookAsync(createDto, token);
                return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Records the return of a borrowed book.
        /// </summary>
        /// <param name="id">The ID of the active borrow record.</param>
        /// <response code="204">Book returned successfully.</response>
        /// <response code="404">If no active (unreturned) record was found for this ID.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id, CancellationToken token = default)
        {
            await _borrowRecordService.ReturnBookAsync(id, token);
    
            return NoContent();
        }

        /// <summary>
        /// Retrieves a list of all borrowing records that have passed their due date.
        /// </summary>
        /// <response code="200">Returns a list of overdue books and the responsible patrons.</response>
        [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetOverdue(CancellationToken token = default)
        {
            var overdueRecords = await _borrowRecordService.GetOverdueRecordsAsync(token);
            return Ok(overdueRecords);
        }
    }
}