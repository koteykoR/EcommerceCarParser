using Microsoft.EntityFrameworkCore;
using CarPrice.Models;

namespace CarPrice.Repository
{
    internal sealed class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public CarContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasKey(c => c.Id);
            modelBuilder.Entity<Car>().Property(c => c.Company);
            modelBuilder.Entity<Car>().Property(c => c.Model);
            modelBuilder.Entity<Car>().Property(c => c.Mileage);
            modelBuilder.Entity<Car>().Property(c => c.EnginePower);
            modelBuilder.Entity<Car>().Property(c => c.EngineVolume);
            modelBuilder.Entity<Car>().Property(c => c.Year);
            modelBuilder.Entity<Car>().Property(c => c.Transmission);
            modelBuilder.Entity<Car>().Property(c => c.Price);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=cardb;Trusted_Connection=True;");
    }
}
