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

        public IQueryable<Book> SearchQuery(string? title, string? author)
        {
            var query = _dbSet
                .Include(b => b.Author)
                .Include(b => b.BorrowRecords)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b =>
                    b.Author.FirstName.Contains(author) ||
                    b.Author.LastName.Contains(author));

            return query;
        }

    }
}
