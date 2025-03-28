using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Queries
{
    public sealed record GetAllBooksQuery(int PageNumber, int PageSize, string SearchFilter) : IRequest<IEnumerable<BookResponseModel>>
    {
    }
}

