using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Persistence.Configurations;

public sealed class MemberTypeEntityConfiguration : IEntityTypeConfiguration<MemberType>
{
    public void Configure(EntityTypeBuilder<MemberType> builder)
    {
        builder.ToTable("MemberType");
        builder.HasKey(x => x.Id);
        builder.HasMany(m => m.Users)
            .WithOne(u => u.MemberType)
            .HasForeignKey(u => u.TypeId);
    }
}
