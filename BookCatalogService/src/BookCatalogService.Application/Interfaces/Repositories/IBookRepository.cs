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
        void Update(Book book);
        void Remove(Book book);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
