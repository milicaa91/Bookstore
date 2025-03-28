using BookCatalogService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using BookDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<BookDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
