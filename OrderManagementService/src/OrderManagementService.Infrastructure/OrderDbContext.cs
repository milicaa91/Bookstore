using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OutboxEvent> OutboxEvents { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Total).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Status).IsRequired();
                entity.Property(o => o.Total).IsRequired();
                entity.Property(o => o.CreatedAt).IsRequired();

                entity.HasMany(o => o.Items)
                   .WithOne()
                   .HasForeignKey(i => i.OrderId) 
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Price).HasColumnType("decimal(18,2)");

                entity.Property(o => o.BookId).IsRequired();
                entity.Property(o => o.Quantity).IsRequired();
                entity.Property(o => o.Price).IsRequired();

                //entity.HasOne<Order>()
                //       .WithMany(o => o.Items)
                //      .HasForeignKey(oi => oi.OrderId)
                //      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OutboxEvent>(entity =>
            {
                entity.HasKey(oi => oi.Id);
            });
        }
    }
}
