using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Persistance.Configurations
{
    public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            
            builder.ToTable("BorrowRecords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BorrowDate)
                .IsRequired();

            builder.Property(x => x.DueDate)
                .IsRequired();

            builder.Property(x => x.ReturnDate);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasOne(x => x.Book)
                .WithMany(x => x.BorrowRecords)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Patron)
                .WithMany(x => x.BorrowRecords)
                .HasForeignKey(x => x.PatronId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.BookId, x.PatronId, x.BorrowDate });
        }
    }
}