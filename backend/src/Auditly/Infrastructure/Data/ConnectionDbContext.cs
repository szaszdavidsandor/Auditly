using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Infrastructure.Data
{
    public class ConnectionDbContext : DbContext
    {
        public ConnectionDbContext(DbContextOptions<ConnectionDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Entrepreneur> Entrepreneurs { get; set; } = null!;
        public DbSet<Accountant> Accountants { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. USER <-> ENTREPRENEUR (1:1 kapcsolat)
            // ==========================================
            modelBuilder.Entity<Entrepreneur>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                      .WithOne(u => u.Entrepreneur)
                      .HasForeignKey<Entrepreneur>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // 2. USER <-> ACCOUNTANT (1:1 kapcsolat)
            // ==========================================
            modelBuilder.Entity<Accountant>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.User)
                      .WithOne(u => u.Accountant)
                      .HasForeignKey<Accountant>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // 3. LOCATION <-> ENTREPRENEUR (1:N kapcsolat)
            // ==========================================
            modelBuilder.Entity<Entrepreneur>(entity =>
            {
                entity.HasOne(e => e.Location)
                      .WithMany(l => l.Entrepreneurs)
                      .HasForeignKey(e => e.LocationId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ==========================================
            // 4. SPECIALIZATION <-> ENTREPRENEUR (1:N kapcsolat)
            // ==========================================
            modelBuilder.Entity<Entrepreneur>(entity =>
            {
                entity.HasOne(e => e.Specialization)
                      .WithMany(s => s.Entrepreneurs)
                      .HasForeignKey(e => e.SpecializationId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}