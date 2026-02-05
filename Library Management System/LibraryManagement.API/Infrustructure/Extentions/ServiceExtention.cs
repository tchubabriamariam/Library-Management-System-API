using LibraryManagement.Infrustructure.Authors;
using LibraryManagement.Infrustructure.Books;
using LibraryManagement.Infrustructure.BorrowRecords;
using LibraryManagement.Infrustructure.Patrons;

namespace Library_Management_System.Infrustructure.Extentions;

public static class ServiceExtention
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<BookRepository>();
        services.AddScoped<AuthorRepository>();
        services.AddScoped<PatronRepository>();
        services.AddScoped<BorrowRecordRepository>();
    }
}