using Company_Printers.api.Models;
using Microsoft.EntityFrameworkCore;
using Printers.api.Models;

namespace Printers.api
{
    public class PrintersDbContext : DbContext
    {
        public PrintersDbContext(DbContextOptions<PrintersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<PrinterMake> PrinterMakes { get; set; }
        public DbSet<Printer> Printers { get; set; } // Renamed to plural for consistency
        public DbSet<Users> Users { get; set; }
        public DbSet<UserLoginResult> UserLoginResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configure the Printer Entity
            modelBuilder.Entity<Printer>(entity =>
            {
                entity.ToTable("Printers");
                // Explicitly set the Primary Key from your SQL script
                entity.HasKey(e => e.EngenPrintersID);
            });

            // 2. Configure PrinterMake
            modelBuilder.Entity<PrinterMake>(entity =>
            {
                entity.ToTable("PrinterMake");
                entity.HasKey(e => e.PrinterMakeID);
            });

            // 3. Configure Designation
            modelBuilder.Entity<Designation>(entity =>
            {
                entity.ToTable("Designation");
                entity.HasKey(e => e.DesignationID);
            });

            // 4. Configure Users
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserID);
            });

            // 5. Mark UserLoginResult as keyless for SP results
            modelBuilder.Entity<UserLoginResult>().HasNoKey();
        }
    }
}