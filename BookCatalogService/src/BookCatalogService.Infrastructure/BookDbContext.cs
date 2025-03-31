using BookCatalogService.Domain.Entities;
using BookCatalogService.Domain.Enums;
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
            modelBuilder.Entity<Book>().HasKey(b => b.Id);

            modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(100);

            modelBuilder.Entity<Book>()
            .Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(200);

            modelBuilder.Entity<Book>()
            .Property(b => b.Category)
            .IsRequired();

            modelBuilder.Entity<Book>()
           .Property(b => b.StockQuantity)
           .IsRequired();

            modelBuilder.Entity<Book>()
              .Property(b => b.Price)
              .HasPrecision(18, 2);

            modelBuilder.Entity<Book>()
              .HasIndex(b => b.Title)
              .HasDatabaseName("IX_Books_Title");

            modelBuilder.Entity<Book>()
            .HasIndex(b => b.Author)
            .HasDatabaseName("IX_Books_Author");

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Category)
                .HasDatabaseName("IX_Books_Category");

            modelBuilder.Entity<Book>()
            .HasIndex(b => new { b.Title, b.Author, b.Category })
            .HasDatabaseName("IX_Books_Title_Author_Category");

            SeedBookData(modelBuilder);
        }

        private void SeedBookData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = new Guid("1182443b-6bc3-400e-a9f6-ff74631b4ee5"), Title = "Clean Code", Author = "Robert C. Martin", Category = Category.Programming, Price = 19.99m, StockQuantity = 50, PublishedAt = new DateTime(2008, 8, 1) },
                new Book { Id = new Guid("1d74449c-c3b9-4452-b08c-10bbe67decbc"), Title = "Design Patterns", Author= "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", Category=Category.Programming, Price = 42.99m, StockQuantity = 30, PublishedAt = new DateTime(1994, 10, 21) },
                new Book { Id = new Guid("4731710e-f850-41ea-b784-060990c98c0c"), Title = "A Game of Thrones", Author="George R.R. Martin", Category=Category.Fiction, Price = 29.99m, StockQuantity = 18,PublishedAt= new DateTime(1996,8,6) },
                new Book { Id = new Guid("642f8f31-f836-4035-97c2-6c7e9aeef8dc"), Title = "Gone Girl", Author= "Gillian Flynn", Category = Category.Mystery, Price = 14.99m, StockQuantity = 18, PublishedAt = new DateTime(1996, 8, 6) },
                new Book { Id = new Guid("67511f83-662a-4ec7-9d34-1e1ebec09b4c"), Title = "The Silence of the Lambs", Category = Category.Mystery, Price = 14.99m, StockQuantity = 18, PublishedAt = new DateTime(1999, 5, 19) },
                new Book { Id = new Guid("6bb27932-6f2b-4353-a1b8-abbf34e06d96"), Title = "To Kill a Mockingbird", Author = "Harper Lee", Category = Category.Literature, Price = 15.99m, StockQuantity = 20, PublishedAt = new DateTime(1960, 7, 11) },
                new Book { Id = new Guid("83972487-6423-4d1b-8c76-2035646fba6d"), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Category = Category.Literature, Price = 13.50m, StockQuantity = 25, PublishedAt = new DateTime(1925, 4, 10) },
                new Book { Id = new Guid("e07821e7-309d-4c8e-b288-531332f59b8f"), Title = "Crime and Punishment", Author = "Fyodor Dostoevsky", Category = Category.Classics, Price = 17.99m, StockQuantity = 5, PublishedAt = new DateTime(1866, 1, 12) },
                new Book { Id = new Guid("ee7d491f-a0dc-4f55-b959-252db34de20f"), Title = "The Godfather", Author = "Mario Puzo", Category = Category.Literature, Price = 21.99m, StockQuantity = 15, PublishedAt = new DateTime(1969, 3, 10) }
            );
        }
    }
}