using BookCatalogService.Application.Features.Books.Commands.DeleteBook;
using BookCatalogService.Application.Interfaces.Repositories;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, string>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book is null)
                throw new BadRequestException($"Book with ID {request.Id} not found");

            book.Title = request.Title;
            book.Author = request.Author;
            book.Category = request.Category;
            book.PublishedAt = request.PublishedAt;
            book.Price = request.Price;
            book.StockQuantity = request.StockQuantity;

            _bookRepository.Update(book);
            await _unitOfWork.SaveChangesAsync();

            return $"Book with ID {request.Id} updated successfully";
        }
    }
}
