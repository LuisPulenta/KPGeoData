using Microsoft.EntityFrameworkCore;
using KPGeoData.Shared.Entities;

namespace KPGeoData.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
