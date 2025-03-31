using BookCatalogService.Application.Features.Books.Queries;
using BookCatalogService.Application.Interfaces.Repositories;
using BookCatalogService.Domain.Entities;
using BookCatalogService.Domain.Enums;
using BookCatalogService.Infrastructure;
using BookCatalogService.Infrastructure.Repositories;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace BookCatalogService.UnitTests
{
    public class BookRepositoryTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BookDbContext _context;
        private readonly IDistributedCache _inMeoryCache;
        private readonly BookRepository _repository;

        public BookRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "TestBookDb")
            .Options;
            _context = new BookDbContext(dbContextOptions);

            var cacheOptions = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            _inMeoryCache = new MemoryDistributedCache(cacheOptions);
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _repository = new BookRepository(_context, _inMeoryCache, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetBookByIdCachedAsync_ShouldReturnCachedBook_WhenCacheExists()
        {
            var bookId = Guid.NewGuid();
            var bookModel = new BookResponseModel(bookId, "Title", "Author", 10.99m, Category.Biography, DateTime.UtcNow, 5);
            var cachedData = JsonSerializer.Serialize(bookModel);

            var cacheKey = $"Book_{bookId}";
            _inMeoryCache.SetString(cacheKey, cachedData);


            var result = await _repository.GetBookByIdCachedAsync(bookId);

            Assert.NotNull(result);
            Assert.Equal(bookModel.Id, result.Id);
        }

        private void MockBooks(BookDbContext _context)
        {
            _context.Books.Add(new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Author = "Author 1",
                Price = 10.99m,
                Category = Category.Biography,
                StockQuantity = 5
            });
            _context.SaveChanges();
        }
    }
}
