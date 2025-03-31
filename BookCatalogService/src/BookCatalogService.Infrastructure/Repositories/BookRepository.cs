using Azure;
using BookCatalogService.Application.Features.Books.Queries;
using BookCatalogService.Application.Interfaces.Repositories;
using BookCatalogService.Domain.Entities;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

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
        public async Task<GetAllBooksResponse> GetPagedBooksAsync(GetAllBooksQuery request)
        {
            IQueryable<Book> query = _context.Books;

            if (!string.IsNullOrWhiteSpace(request.Title))
                query = query.Where(b => b.Title.Contains(request.Title));

            if (!string.IsNullOrWhiteSpace(request.Author))
                query = query.Where(b => b.Author.Contains(request.Author));

            if (request.Category != 0)
                query = query.Where(b => b.Category == request.Category);

            query = request.SortOrder?.ToLower() == "desc" ? query.OrderByDescending(GetSortProperty(request)) : query.OrderBy(GetSortProperty(request));

            var totalCount = await query.CountAsync();

            var books = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                   .Take(request.PageSize).ToListAsync();

            var response = new GetAllBooksResponse
            {
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
                Page = request.PageNumber,
                PageSize = request.PageSize,
                Books = books
            };

            return response;
        }

        private Expression<Func<Book, object>> GetSortProperty(GetAllBooksQuery request)
        {
            return request.SortColumn switch
            {
                "id" => b => b.Id,
                "title" => b => b.Title,
                "author" => b => b.Author,
                "category" => b => b.Category,
                "price" => b => b.Price,
                _ => b => b.Title
            };
        }

        public async Task<BookResponseModel> GetBookByIdCachedAsync(Guid bookId)
        {
            var cacheKey = $"Book_{bookId}";

            var cachedBook = await _cache.GetStringAsync(cacheKey);

            if (cachedBook is not null)
                return JsonSerializer.Deserialize<BookResponseModel>(cachedBook);

            var book = await base.GetByIdAsync(bookId);

            if (book is null)
                throw new KeyNotFoundException($"Book with id {bookId} not found.");

            var bookModel = new BookResponseModel(book.Id, 
                book.Title, 
                book.Author, 
                book.Price, 
                book.Category,
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
