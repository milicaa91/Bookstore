﻿using BookCatalogService.Application.Interfaces.Repositories;
using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Queries
{
    public class GetBookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, BookResponseModel>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookDetailsQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        public async Task<BookResponseModel> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request is null || request.Id == Guid.Empty)
                throw new ArgumentNullException($"Book details can't be fetched");

            //TODO MN from cache
            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book is null)
                throw new NotFoundException($"Book with ID {request.Id} not found");

            return new BookResponseModel(
                 book.Id,
                 book.Title,
                 book.Author,
                 book.Price,
                 book.PublishedAt,
                 book.StockQuantity
             );
        }
    }
}
