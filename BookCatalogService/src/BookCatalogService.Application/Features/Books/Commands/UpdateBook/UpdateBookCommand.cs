using BookCatalogService.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.UpdateBook
{
    public record UpdateBookCommand(Guid Id,string Title, string Author, Category Category, DateTime PublishedAt, decimal Price, int StockQuantity) : IRequest<string>;
}
