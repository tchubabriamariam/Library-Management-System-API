using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Persistance.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ISBN)
                .IsRequired()
                .IsUnicode(false)     
                .HasMaxLength(20);

            builder.HasIndex(x => x.ISBN).IsUnique();

            builder.Property(x => x.PublicationYear);

            builder.Property(x => x.Description)
                .HasMaxLength(4000);

            builder.Property(x => x.CoverImageUrl)
                .IsUnicode(false)
                .HasMaxLength(500);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}