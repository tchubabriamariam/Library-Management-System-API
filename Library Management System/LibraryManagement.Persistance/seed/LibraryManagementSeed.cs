using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Persistance.Context;

namespace LibraryManagement.Persistance.Seed
{
    public static class LibraryManagementSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // this is main method and calls every other method and aslo saves changes so all values are saved in database
            
            using var scope = serviceProvider.CreateAsyncScope();

            var db = scope.ServiceProvider.GetRequiredService<LibraryManagementContext>();

            db.Database.Migrate();

            SeedAuthors(db);
            db.SaveChanges();

            SeedBooks(db);
            db.SaveChanges();

            SeedPatrons(db);
            db.SaveChanges();

            SeedBorrowRecords(db);
            db.SaveChanges();
        }

        private static void SeedAuthors(LibraryManagementContext context)
        {
            // adding authors but they shouldnt exist in database
            var authors = new List<Author>
            {
                new Author { FirstName = "George", LastName = "Orwell", Biography = "English novelist and essayist." },
                new Author { FirstName = "Jane", LastName = "Austen", Biography = "English novelist known for realism." }
            };

            foreach (var author in authors)
            {
                var exists = context.Authors.Any(x =>
                    x.FirstName == author.FirstName &&
                    x.LastName == author.LastName);

                if (exists) continue;

                context.Authors.Add(author);
            }
        }

        private static void SeedBooks(LibraryManagementContext context)
        { 
            // adding books
            var orwell = context.Authors.FirstOrDefault(x => x.FirstName == "George" && x.LastName == "Orwell");
            var austen = context.Authors.FirstOrDefault(x => x.FirstName == "Jane" && x.LastName == "Austen");

            if (orwell == null || austen == null) return;

            var books = new List<Book>
            {
                new Book
                {
                    Title = "1984",
                    ISBN = "9780451524935",
                    PublicationYear = 1949,
                    Description = "Dystopian novel.",
                    Quantity = 5,
                    AuthorId = orwell.Id
                },
                new Book
                {
                    Title = "Pride and Prejudice",
                    ISBN = "9780141439518",
                    PublicationYear = 1813,
                    Description = "Classic romance novel.",
                    Quantity = 3,
                    AuthorId = austen.Id
                }
            };

            foreach (var book in books)
            {
                var exists = context.Books.Any(x => x.ISBN == book.ISBN);
                if (exists) continue;

                context.Books.Add(book);
            }
        }

        private static void SeedPatrons(LibraryManagementContext context)
        {
            var patrons = new List<Patron>
            {
                new Patron
                {
                    FirstName = "Mariam",
                    LastName = "Barbakadze",
                    Email = "mariam@example.com",
                    MembershipDate = DateTime.UtcNow.AddMonths(-2)
                },
                new Patron
                {
                    FirstName = "Nika",
                    LastName = "K.",
                    Email = "nika@example.com",
                    MembershipDate = DateTime.UtcNow.AddMonths(-1)
                }
            };

            foreach (var patron in patrons)
            {
                var exists = context.Patrons.Any(x => x.Email == patron.Email);
                if (exists) continue;

                context.Patrons.Add(patron);
            }
        }

        private static void SeedBorrowRecords(LibraryManagementContext context)
        {
            // one borrowing record
            var patron = context.Patrons.OrderBy(x => x.Id).FirstOrDefault();
            var book = context.Books.OrderBy(x => x.Id).FirstOrDefault();

            if (patron == null || book == null) return; // if data isnt there stop

            var exists = context.BorrowRecords.Any(x =>
                x.BookId == book.Id &&
                x.PatronId == patron.Id &&
                x.ReturnDate == null);

            if (exists) return;

            var record = new BorrowRecord
            {
                BookId = book.Id,
                PatronId = patron.Id,
                BorrowDate = DateTime.UtcNow.AddDays(-3),
                DueDate = DateTime.UtcNow.AddDays(11),
                ReturnDate = null,
                Status = BorrowStatus.Borrowed
            };

            context.BorrowRecords.Add(record);
        }
    }
}
