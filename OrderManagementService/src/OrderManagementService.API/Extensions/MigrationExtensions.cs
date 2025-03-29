using Microsoft.EntityFrameworkCore;
using OrderManagementService.Infrastructure;

namespace OrderManagementService.API.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using OrderDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
