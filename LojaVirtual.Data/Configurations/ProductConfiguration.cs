using LojaVirtual.Entities.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.CategoryId);

        }
    }
}
