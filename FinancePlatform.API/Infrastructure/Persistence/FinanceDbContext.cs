using FinancePlatform.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancePlatform.API.Infrastructure.Persistence
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Reconciliation> Reconciliations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.HolderName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.AccountNumber).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Balance).HasPrecision(18, 2); 
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Amount).HasPrecision(18, 2);
                entity.Property(p => p.CreatedAt).IsRequired();
                entity.Property(p => p.Status).IsRequired();

                entity.HasOne<Account>()
                      .WithMany()
                      .HasForeignKey(p => p.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Reconciliation>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.ProcessedAt).IsRequired();
                entity.Property(r => r.IsSuccessful).IsRequired();

                entity.HasOne<Payment>()
                      .WithMany()
                      .HasForeignKey(r => r.PaymentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message).IsRequired().HasMaxLength(255);
                entity.Property(n => n.SentAt).IsRequired();
                entity.Property(n => n.Type).IsRequired();

                entity.HasOne<Account>()
                      .WithMany()
                      .HasForeignKey(n => n.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
