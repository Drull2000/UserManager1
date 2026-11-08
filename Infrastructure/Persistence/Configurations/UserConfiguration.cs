using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManager.Domain.Entities;

namespace UserManager.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Name).IsRequired().HasMaxLength(100);
        builder.Property(user => user.Email).IsRequired().HasMaxLength(254);
        builder.Property(user => user.PasswordHash).IsRequired().HasMaxLength(500);
        builder.Property(user => user.CreatedDate).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();
    }
}
