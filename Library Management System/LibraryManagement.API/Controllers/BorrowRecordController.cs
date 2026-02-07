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

        [HttpGet]
        public async Task<ActionResult<PagedResult<BorrowRecordDto>>> GetAll([FromQuery] BorrowRecordFilterDto filter, CancellationToken ct = default)
        {
            var result = await _borrowRecordService.GetAllAsync(filter, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecordDto>> GetById(int id, CancellationToken ct = default)
        {
            var record = await _borrowRecordService.GetByIdAsync(id, ct);
            
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<BorrowRecordDto>> CheckOut([FromBody] CreateBorrowRecordDto createDto, CancellationToken ct = default)
        {
            try
            {
                var record = await _borrowRecordService.CheckOutBookAsync(createDto, ct);
                return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id, CancellationToken ct = default)
        {
            var success = await _borrowRecordService.ReturnBookAsync(id, ct);
            
            if (!success)
                return NotFound(new { message = "Active borrow record not found or already returned." });

            return NoContent();
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetOverdue(CancellationToken ct = default)
        {
            var overdueRecords = await _borrowRecordService.GetOverdueRecordsAsync(ct);
            return Ok(overdueRecords);
        }
    }
}