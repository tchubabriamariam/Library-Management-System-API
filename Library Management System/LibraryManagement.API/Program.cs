using Library_Management_System.Infrustructure.Extentions;
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

builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();