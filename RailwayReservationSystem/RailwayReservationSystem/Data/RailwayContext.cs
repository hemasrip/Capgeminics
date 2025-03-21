using Microsoft.EntityFrameworkCore;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Data
{
    public class RailwayContext : DbContext
    {
        public RailwayContext(DbContextOptions<RailwayContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Quota> Quotas { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints if needed
            modelBuilder.Entity<User>()
                .HasMany(u => u.Passengers)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<Passenger>()
                .HasMany(p => p.Reservations)
                .WithOne(r => r.Passenger)
                .HasForeignKey(r => r.PassengerID);

            modelBuilder.Entity<Train>()
                .HasMany(t => t.Quotas)
                .WithOne(q => q.Train)
                .HasForeignKey(q => q.TrainID);

            modelBuilder.Entity<Train>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Train)
                .HasForeignKey(r => r.TrainID);

            modelBuilder.Entity<Quota>()
                .HasMany(q => q.Reservations)
                .WithOne(r => r.Quota)
                .HasForeignKey(r => r.QuotaID);
        }
    }
}
