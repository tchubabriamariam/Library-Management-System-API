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
            using var scope = serviceProvider.CreateAsyncScope();

            var database = scope.ServiceProvider.GetRequiredService<LibraryManagementContext>();

            Migrate(database);

            SeedEverything(database);
        }

        private static void Migrate(LibraryManagementContext context)
        {
            context.Database.Migrate();
        }

        private static void SeedEverything(LibraryManagementContext context)
        {
            var seeded = false;

            SeedAuthors(context, ref seeded);
            SeedBooks(context, ref seeded);
            SeedPatrons(context, ref seeded);
            SeedBorrowRecords(context, ref seeded);

            if (seeded)
                context.SaveChanges();
        }

        private static void SeedAuthors(LibraryManagementContext context, ref bool seeded)
        {
            var authors = new List<Author>
            {
                new Author { FirstName = "George", LastName = "Orwell", Biography = "English novelist and essayist." },
                new Author { FirstName = "Jane", LastName = "Austen", Biography = "English novelist known for realism." }
            };

            foreach (var author in authors)
            {
                if (context.Authors.Any(x => x.FirstName == author.FirstName && x.LastName == author.LastName))
                    continue;

                context.Authors.Add(author);
                seeded = true;
            }
        }

        private static void SeedBooks(LibraryManagementContext context, ref bool seeded)
        {
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
                if (context.Books.Any(x => x.ISBN == book.ISBN))
                    continue;

                context.Books.Add(book);
                seeded = true;
            }
        }

        private static void SeedPatrons(LibraryManagementContext context, ref bool seeded)
        {
            var patrons = new List<Patron>
            {
                new Patron { FirstName = "Mariam", LastName = "Barbakadze", Email = "mariam@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-2) },
                new Patron { FirstName = "Nika", LastName = "K.", Email = "nika@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-1) }
            };

            foreach (var patron in patrons)
            {
                if (context.Patrons.Any(x => x.Email == patron.Email))
                    continue;

                context.Patrons.Add(patron);
                seeded = true;
            }
        }

        private static void SeedBorrowRecords(LibraryManagementContext context, ref bool seeded)
        {
            var patron = context.Patrons.FirstOrDefault();
            var book = context.Books.FirstOrDefault();

            if (patron == null || book == null) return;

            var exists = context.BorrowRecords.Any(x => x.BookId == book.Id && x.PatronId == patron.Id && x.ReturnDate == null);
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
            seeded = true;
        }
    }
}
