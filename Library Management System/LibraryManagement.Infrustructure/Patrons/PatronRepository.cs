using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Repositories;
using LibraryManagement.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrustructure.Patrons
{
    public class PatronRepository : BaseRepository<Patron>
    {
        public PatronRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task<List<Patron>> GetAllWithBorrowRecordsAsync(CancellationToken token)
        {
            return await _dbSet
                .Include(x => x.BorrowRecords)
                .ThenInclude(x => x.Book)
                .ToListAsync(token);
        }

        public async Task<Patron?> GetWithBorrowRecordsAsync(CancellationToken token, int id)
        {
            return await _dbSet
                .Include(x => x.BorrowRecords)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }
    }
}