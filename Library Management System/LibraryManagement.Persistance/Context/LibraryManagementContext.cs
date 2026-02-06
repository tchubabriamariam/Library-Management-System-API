using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Persistance.Context
{
    public class LibraryManagementContext : DbContext
    {
        #region Ctor

        public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
            : base(options)
        {
        }
        
        #endregion

        #region DbSets 
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Patron> Patrons { get; set; } = null!;
        public DbSet<BorrowRecord> BorrowRecords { get; set; } = null!;

        #endregion

        #region Configurations

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryManagementContext).Assembly);
        }

        #endregion
        
        

        // main method
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries()
                             .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
                {
                    entry.State = EntityState.Detached;
                }
                throw;            
            }
        }
    }
}
