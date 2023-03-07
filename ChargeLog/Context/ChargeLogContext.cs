using ChargeLog.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ChargeLog.Context
{
    public class ChargeLogContext : DbContext
    {
        public DbSet<Network> Networks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Import> Imports { get; set; }

        public ChargeLogContext(DbContextOptions<ChargeLogContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //  Network table setup

            builder.Entity<Network>()
            .ToTable("Network");

            builder.Entity<Network>()
                .Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Location table setup 

            builder.Entity<Location>()
            .ToTable("Location");

            builder.Entity<Location>()
                .Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Entity<Location>()
                .Property(l => l.Address)
                .HasMaxLength(150);

            // session table setup 

            builder.Entity<Session>()
            .ToTable("Session");

            // Car table setup 

            builder.Entity<Car>()
            .ToTable("Car");

            builder.Entity<Car>()
                .Property(c => c.Year)
                .IsRequired()
                .HasMaxLength(10);

            builder.Entity<Car>()
                .Property(c => c.Make)
                .IsRequired()
                .HasMaxLength(50);

            builder.Entity<Car>()
               .Property(c => c.Model)
               .IsRequired()
               .HasMaxLength(50);

            // Group table setup 

            builder.Entity<Group>()
            .ToTable("Group");

            builder.Entity<Group>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Import table setup 

            builder.Entity<Import>()
                .ToTable("Import");

            builder.Entity<Import>()
                .Property(i => i.Type)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
