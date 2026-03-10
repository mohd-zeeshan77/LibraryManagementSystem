using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Persistence.Configurations;

public sealed class BookEntityConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Book");
        builder.HasKey(c => c.Id);
        builder
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(b => b.AuthorName)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(b => b.Publisher)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(b => b.Edition)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId);
        builder.HasMany(b => b.IssuedBooks)
            .WithOne(i => i.Book)
            .HasForeignKey(b => b.BookId);
    }
}
