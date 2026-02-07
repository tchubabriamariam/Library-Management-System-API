using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Patrons;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Authors;
using LibraryManagement.Infrustructure.Books;
using LibraryManagement.Infrustructure.BorrowRecords;
using LibraryManagement.Infrustructure.Patrons;
using LibraryManagement.Infrustructure.Repositories;

namespace Library_Management_System.Infrustructure.extentions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<BookRepository>();
        services.AddScoped<AuthorRepository>();
        services.AddScoped<PatronRepository>();
        services.AddScoped<BorrowRecordRepository>();
        services.AddScoped<BaseRepository<Book>>();
        services.AddScoped<BaseRepository<Author>>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IPatronService, PatronService>();
    }
}