using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Data
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<TableEntity> Tables => Set<TableEntity>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(10, 2);


            // TableNumber unic (nu ai 2 mese cu același număr)
            modelBuilder.Entity<TableEntity>()
                .HasIndex(t => t.TableNumber)
                .IsUnique();

            // Relație: Client 1 - M Reservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relație: TableEntity 1 - M Reservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.TableEntity)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TableEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relație: Reservation 1 - 0..1 Review (one-to-one)
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Reservation)
                .WithOne(r => r.Review)
                .HasForeignKey<Review>(rv => rv.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relație: Client 1 - M Reviews
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Client)
                .WithMany(c => c.Reviews)
                .HasForeignKey(rv => rv.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review unic per rezervare (deja e one-to-one, dar îl întărește)
            modelBuilder.Entity<Review>()
                .HasIndex(r => r.ReservationId)
                .IsUnique();
        }
    }
}
