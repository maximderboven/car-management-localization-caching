using System.Diagnostics;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Insurance.Domain;

namespace Insurance.DAL.EF
{
    public class InsuranceDbContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public InsuranceDbContext()
        {
            //rebuild bool: opnieuw db droppen en seeden (1x maar nodig)
            InsuranceInitializer.Initialize(this,false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // configure lazy-loading
            //optionsBuilder.UseLazyLoadingProxies(false);
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlite(@$"Data Source={Directory.GetCurrentDirectory()}\..\database.db");
            optionsBuilder.LogTo(p => Debug.WriteLine(p), LogLevel.Information);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tussentabel Rental
            // 0..1-n
            // Driver kan met een auto rijden voor een bepaalde periode (rental x insurance)

            modelBuilder.Entity<Rental>().ToTable("Rentals");

            //de veel op veel tussentabel
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Driver)
                .WithMany(d => d.Rentals)
                .HasForeignKey("FK_DriverID")
                .IsRequired();
            
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey("FK_CarID")
                .IsRequired();

            modelBuilder.Entity<Rental>()
                .HasKey(new string[] {"FK_DriverID", "FK_CarID"});
            
            
            //de Car - Garage een op veel
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Garage)
                .WithMany(g => g.Cars)
                .HasForeignKey("GarageID")
                .IsRequired();
        }
    }
}