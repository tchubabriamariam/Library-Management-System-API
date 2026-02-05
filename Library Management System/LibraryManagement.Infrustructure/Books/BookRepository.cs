using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Repositories;
using LibraryManagement.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrustructure.Books
{
    public class BookRepository : BaseRepository<Book>
    {
        public BookRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task<List<Book>> SearchAsync(CancellationToken token, string? title, string? author)
        {
            var query = _dbSet
                .Include(x => x.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(x => x.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(x =>
                    (x.Author.FirstName + " " + x.Author.LastName).Contains(author));

            return await query.ToListAsync(token);
        }
    }
}
