using BookCatalogService.Application.Interfaces.Repositories;
using BookCatalogService.Domain.Entities;
using Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Category = request.Category,
                PublishedAt = request.PublishedAt,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            await _bookRepository.AddAsync(book);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return book.Id;
        }
    }
}
