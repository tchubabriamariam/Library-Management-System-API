using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Persistance.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Biography)
                .HasMaxLength(2000);

            builder.Property(x => x.DateOfBirth);

            builder.HasMany(x => x.Books)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}