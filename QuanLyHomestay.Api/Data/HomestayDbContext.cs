using Microsoft.EntityFrameworkCore;
using QuanLyHomestay.Api.Models;

namespace QuanLyHomestay.Api.Data;

public class HomestayDbContext(DbContextOptions<HomestayDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer).WithMany()
            .HasForeignKey(b => b.CustomerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room).WithMany()
            .HasForeignKey(b => b.RoomId).OnDelete(DeleteBehavior.Restrict);
    }
}
