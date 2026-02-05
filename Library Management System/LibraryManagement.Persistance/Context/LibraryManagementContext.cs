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
                var entries = ChangeTracker
                    .Entries()
                    .Where(e => e.Entity != null &&
                                (e.State == EntityState.Modified ||
                                 e.State == EntityState.Added ||
                                 e.State == EntityState.Deleted))
                    .ToList();

                foreach (var entry in entries)
                {
                    entry.State = entry.State switch
                    {
                        EntityState.Added => EntityState.Modified,
                        _ => EntityState.Unchanged
                    };
                }

                throw;
            }
        }
    }
}
