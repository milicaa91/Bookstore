using BookCatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Infrastructure
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.Id); //TODO MN add HasKey

            //modelBuilder.Entity<Book>().HasData(
            //    new Book { Id = Guid.NewGuid(), Title = "The Hobbit", Price = 19.99m, StockQuantity = 50 },
            //    new Book { Id = Guid.NewGuid(), Title = "1984", Price = 14.99m, StockQuantity = 30 }
            //);
        }
    }
}