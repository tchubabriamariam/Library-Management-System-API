using LibraryManagement.Application.Books;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Infrustructure.Authors;
using LibraryManagement.Infrustructure.Books;
using LibraryManagement.Infrustructure.BorrowRecords;
using LibraryManagement.Infrustructure.Patrons;
using LibraryManagement.Infrustructure.Repositories;
using LibraryManagement.Persistance;
using LibraryManagement.Persistance.Context;
using Microsoft.EntityFrameworkCore;
// removed extra stuff
var builder = WebApplication.CreateBuilder(args);

// controllers
builder.Services.AddControllers();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF is here
builder.Services.AddDbContext<LibraryManagementContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(ConnectionString.DefaultConnection)));

builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<PatronRepository>();
builder.Services.AddScoped<BorrowRecordRepository>();
builder.Services.AddScoped<BaseRepository<Book>>();
builder.Services.AddScoped<IBookService, BookService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();