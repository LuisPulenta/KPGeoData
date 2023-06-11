using Microsoft.EntityFrameworkCore;
using KPGeoData.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KPGeoData.API.Data
{
        public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemPhoto> ItemPhotos { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyEventType> SurveyEventTypes { get; set; }
        public DbSet<SurveyItemType> SurveyItemTypes { get; set; }
        public DbSet<SurveyState> SurveyStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Survey>().HasIndex("Name", "CompanyId").IsUnique();
            modelBuilder.Entity<Item>().HasIndex("Name", "SurveyId").IsUnique();
            modelBuilder.Entity<ItemType>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<EventType>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<SurveyEventType>().HasIndex("EventType", "SurveyId").IsUnique();
            modelBuilder.Entity<SurveyItemType>().HasIndex("ItemType", "SurveyId").IsUnique();
            modelBuilder.Entity<SurveyState>().HasIndex("State", "SurveyId").IsUnique();
        }
    }
}
