using BookCatalogService.Application.Interfaces.Repositories;
using BookCatalogService.Infrastructure.Repositories;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly(typeof(BookDbContext).Assembly.FullName)));

            services.AddScoped<IBookRepository, BookRepository>(); // Register specific repository
            services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register specific repository

            return services;
        }
    }
}
