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

            // ==========================================
            // 5. USER <-> LOCATION (Cascade Delete)
            // ==========================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Location)
                      .WithMany()
                      .HasForeignKey(u => u.LocationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // 6. USER EMAIL UNIQUE
            // ==========================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // ==========================================
            // 6. Entrepreneur unique kulcsok
            // ==========================================
            modelBuilder.Entity<Entrepreneur>(entity =>
            {
                entity.HasIndex(e => e.Iban).IsUnique();
                entity.HasIndex(e => e.TaxNumber).IsUnique();
                entity.HasIndex(e => e.CompanyName).IsUnique();
                entity.HasIndex(e => e.Phone).IsUnique();
            }
            );

            // ==========================================
            // 6. Accountant unique kulcsok
            // ==========================================
            modelBuilder.Entity<Accountant>(entity =>
            {
                entity.HasIndex(e => e.CompanyName).IsUnique();
                entity.HasIndex(e => e.Phone).IsUnique();
                entity.HasIndex(e => e.RegistrationNumber).IsUnique();

            }
            );



            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(rt => rt.User)
                      .WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // VerificationToken -> User kapcsolat (Kaskádolt törléssel)
            modelBuilder.Entity<VerificationToken>(entity =>
            {
                entity.HasOne(vt => vt.User)
                      .WithMany()
                      .HasForeignKey(vt => vt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}