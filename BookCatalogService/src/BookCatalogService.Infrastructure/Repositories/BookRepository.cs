using BookCatalogService.Application.Features.Books.Queries;
using BookCatalogService.Application.Interfaces.Repositories;
using BookCatalogService.Domain.Entities;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookCatalogService.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book, Guid>, IBookRepository
    {
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _unitOfWork;

        public BookRepository(BookDbContext context, IDistributedCache cache, 
            IUnitOfWork unitOfWork) : base(context)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(b => b.Id == id);
        }

        public async Task<BookResponseModel> GetBookByIdCachedAsync(Guid bookId)
        {
            var cacheKey = $"Book_{bookId}";

            var cachedBook = await _cache.GetStringAsync(cacheKey);

            if (cachedBook is not null)
                return JsonSerializer.Deserialize<BookResponseModel>(cachedBook);

            var book = base.GetByIdAsync(bookId).Result;

            if (book is null)
                return null;

            var bookModel = new BookResponseModel(book.Id, 
                book.Title, 
                book.Author, 
                book.Price, 
                book.PublishedAt, 
                book.StockQuantity);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(bookModel), cacheOptions);

            return bookModel;
        }

        public async Task UpdateAsync(Book book)
        {
            base.Update(book);
            await _unitOfWork.SaveChangesAsync();
            await _cache.RemoveAsync($"Book_{book.Id}");
        }

        public async Task DeleteAsync(Book book)
        {
            base.Remove(book);
            await _unitOfWork.SaveChangesAsync();
            await _cache.RemoveAsync($"Book_{book.Id}");
        }
    }
}
