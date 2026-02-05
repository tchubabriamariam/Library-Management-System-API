using LibraryManagement.Domain.Entity;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrustructure.Repositories;
using LibraryManagement.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrustructure.BorrowRecords
{
    public class BorrowRecordRepository : BaseRepository<BorrowRecord>
    {
        public BorrowRecordRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task<List<BorrowRecord>> GetAllWithDetailsAsync(CancellationToken token)
        {
            return await _dbSet
                .Include(x => x.Book)
                .Include(x => x.Patron)
                .ToListAsync(token);
        }

        public async Task<List<BorrowRecord>> GetOverdueAsync(CancellationToken token)
        {
            var today = DateTime.UtcNow;

            return await _dbSet
                .Include(x => x.Book)
                .Include(x => x.Patron)
                .Where(x => x.ReturnDate == null && x.DueDate < today)
                .ToListAsync(token);
        }

        public async Task<BorrowRecord?> GetActiveBorrowAsync(CancellationToken token, int bookId, int patronId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x =>
                        x.BookId == bookId &&
                        x.PatronId == patronId &&
                        x.ReturnDate == null,
                    token);
        }
    }
}