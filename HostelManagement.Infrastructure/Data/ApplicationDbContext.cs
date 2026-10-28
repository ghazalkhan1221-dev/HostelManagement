using global::HostelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HostelManagement.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hostel> Hostels => Set<Hostel>();

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Bed> Beds => Set<Bed>();

    public DbSet<Resident> Residents => Set<Resident>();

    public DbSet<ResidentAllocation> ResidentAllocations
        => Set<ResidentAllocation>();

    public DbSet<FeeCharge> FeeCharges => Set<FeeCharge>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Complaint> Complaints => Set<Complaint>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hostel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HostelId);

        modelBuilder.Entity<Bed>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Beds)
            .HasForeignKey(b => b.RoomId);

        modelBuilder.Entity<ResidentAllocation>()
            .HasOne(a => a.Resident)
            .WithMany()
            .HasForeignKey(a => a.ResidentId);

        modelBuilder.Entity<ResidentAllocation>()
            .HasOne(a => a.Bed)
            .WithMany()
            .HasForeignKey(a => a.BedId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.FeeCharge)
            .WithMany()
            .HasForeignKey(p => p.FeeChargeId);

        modelBuilder.Entity<Complaint>()
            .HasOne(c => c.Resident)
            .WithMany()
            .HasForeignKey(c => c.ResidentId);
    }
}