using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Persistence.Configurations;

public sealed class IssuedBookEntityConfiguration : IEntityTypeConfiguration<IssuedBook>
{
    public void Configure(EntityTypeBuilder<IssuedBook> builder)
    {
        builder.ToTable("IssuedBook");
        builder.HasKey(x => x.Id);
        builder.HasOne(i => i.User)
            .WithMany(u => u.IssuedBooks)
            .HasForeignKey(i => i.UserId);
        builder.HasOne(i => i.Book)
            .WithMany(b => b.IssuedBooks)
            .HasForeignKey(i => i.BookId);
    }
}
