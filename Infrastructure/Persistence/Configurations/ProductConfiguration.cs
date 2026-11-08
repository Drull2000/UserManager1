using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManager.Domain.Entities;

namespace UserManager.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(product => product.Id);
        builder.Property(product => product.Name).IsRequired().HasMaxLength(100);
        builder.Property(product => product.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(product => product.Description).HasMaxLength(500);
        builder.Property(product => product.Quantity).IsRequired();
        builder.Property(product => product.CreatedDate).IsRequired();
        builder.Property(product => product.IsDeleted).IsRequired();
        builder.HasIndex(product => product.Name);
    }
}
