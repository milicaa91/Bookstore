using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookDbContext _context;

        public UnitOfWork(BookDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync()
        {
            await Task.Run(() => _context.ChangeTracker.Clear());
        }
    }
}
