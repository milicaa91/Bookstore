﻿using BookCatalogService.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.AddBook
{
    public record AddBookCommand(string Title, string Author, Category Category, DateTime PublishedAt, decimal Price, int StockQuantity) : IRequest<Guid>;
}
