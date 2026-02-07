using Library_Management_System.Infrustructure.extentions;
using LibraryManagement.API.Infrastructure.middlewares;
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

// services outside like we learned in extentions folder
builder.Services.AddServices();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

try
{
    LibraryManagement.Persistance.Seed.LibraryManagementSeed.Initialize(app.Services); // for first values in database
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}