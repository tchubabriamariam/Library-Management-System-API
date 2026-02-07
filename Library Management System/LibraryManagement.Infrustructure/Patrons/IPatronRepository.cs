using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Infrustructure.Patrons;

public interface IPatronRepository
{
    Task<List<Patron>> GetAllWithBorrowRecordsAsync(CancellationToken token);
    Task<Patron?> GetWithBorrowRecordsAsync(CancellationToken token, int id);
}