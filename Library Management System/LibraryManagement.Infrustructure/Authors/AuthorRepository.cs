using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Repositories;
using LibraryManagement.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrustructure.Authors
{
    public class AuthorRepository : BaseRepository<Author>
    {
        public AuthorRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task<List<Author>> GetAllWithBooksAsync(CancellationToken token)
        {
            return await _dbSet
                .Include(x => x.Books)
                .ToListAsync(token);
        }

        public async Task<Author?> GetWithBooksAsync(CancellationToken token, int id)
        {
            return await _dbSet
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }
    }
}