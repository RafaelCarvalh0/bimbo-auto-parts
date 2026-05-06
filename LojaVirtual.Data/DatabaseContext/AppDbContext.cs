using LojaVirtual.Entities.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
