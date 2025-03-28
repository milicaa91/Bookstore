using BookCatalogService.Application.Features.Books.Commands.AddBook;
using BookCatalogService.Application.Interfaces.Repositories;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, string>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book is null) 
                throw new BadRequestException($"Book with ID {request.Id} not found");

             _bookRepository.Remove(book);
            await _unitOfWork.SaveChangesAsync();

            return $"Book with ID {request.Id} deleted successfully";
        }
    }
}
