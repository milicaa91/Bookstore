using BookCatalogService.Application.Interfaces.Repositories;
using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Queries
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, GetAllBooksResponse>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<GetAllBooksResponse> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0 || request.PageNumber <= 0)
                throw new BadRequestException("Page index and page size should be greater than zero!");

            return await _bookRepository.GetPagedBooksAsync(request);
        }
    }
}
