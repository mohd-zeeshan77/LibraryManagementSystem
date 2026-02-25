using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Persistence.Configurations;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasMany(u => u.IssuedBooks)
            .WithOne(i => i.User)
            .HasForeignKey(u => u.UserId);
        builder.HasOne(u => u.memberType)
            .WithMany(m => m.Users)
            .HasForeignKey(u => u.TypeId);
    }
}
