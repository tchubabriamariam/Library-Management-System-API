using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Persistance.Configurations
{
    public class PatronConfiguration : IEntityTypeConfiguration<Patron>
    {
        public void Configure(EntityTypeBuilder<Patron> builder)
        {
            builder.ToTable("Patrons");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(256);

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.MembershipDate)
                .IsRequired();
        }
    }
}