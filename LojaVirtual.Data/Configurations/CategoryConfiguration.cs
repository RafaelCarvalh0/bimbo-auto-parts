using LojaVirtual.Entities.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
