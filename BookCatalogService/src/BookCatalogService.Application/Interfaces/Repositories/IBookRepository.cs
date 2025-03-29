﻿using BookCatalogService.Application.Features.Books.Queries;
using BookCatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task<BookResponseModel> GetBookByIdCachedAsync(Guid bookId);
        Task<bool> ExistsAsync(Guid id);
    }
}
