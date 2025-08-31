using Microsoft.EntityFrameworkCore;
using QuickPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace QuickPay.Infrastructure.Contexts
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Payment entity configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Amount).HasPrecision(18, 2);
                entity.Property(p => p.Currency).IsRequired().HasMaxLength(3);
                entity.Property(p => p.CardLast4).HasMaxLength(4);
                entity.Property(p => p.Status).IsRequired();
                entity.Property(p => p.Reason).HasMaxLength(255);
            });
        }
    }
}
