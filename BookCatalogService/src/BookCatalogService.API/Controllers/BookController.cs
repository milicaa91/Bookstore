using Azure.Core;
using BookCatalogService.Application.Features.Books.Commands.AddBook;
using BookCatalogService.Application.Features.Books.Commands.DeleteBook;
using BookCatalogService.Application.Features.Books.Commands.UpdateBook;
using BookCatalogService.Application.Features.Books.Queries;
using BookCatalogService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Threading;

namespace BookCatalogService.API.Controllers
{
    [Authorize(Policy = "AdminOperatorPolicy")]
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly ISender _sender;
        public BookController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Policy = "AdminOperatorUserPolicy")]
        [HttpGet]
        public async Task<ActionResult> GetAllBooks([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, 
            [FromQuery] string title = null, 
            [FromQuery] string author = null, 
            [FromQuery] Category category = 0, 
            [FromQuery] string sortColumn = null, 
            [FromQuery] string sortOrder = null, 
            CancellationToken cancellationToken = default )
        {
            var query = new GetAllBooksQuery(pageNumber, pageSize, title,author, category, sortColumn, sortOrder);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOperatorUserPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseModel>> GetBookById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookDetailsQuery(id);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddBook([FromBody] AddBookRequest addBookRequest, CancellationToken cancellationToken)
        {
            var addBookCommand = new AddBookCommand(addBookRequest.Title, addBookRequest.Author, addBookRequest.Category,
                 addBookRequest.PublishedAt, addBookRequest.Price, addBookRequest.StockQuantity);

            var result = await _sender.Send(addBookCommand, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(Guid id, [FromBody] UpdateBookRequest updateBookRequest, 
            CancellationToken cancellationToken)
        {
            var updateBookCommand = new UpdateBookCommand(id, updateBookRequest.Title, updateBookRequest.Author, updateBookRequest.Category,
                 updateBookRequest.PublishedAt, updateBookRequest.Price, updateBookRequest.StockQuantity);

            var result = await _sender.Send(updateBookCommand, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
        {
            var deleteBookCommand = new DeleteBookCommand(id);

            var result = await _sender.Send(deleteBookCommand, cancellationToken);
            return Ok(result);
        }
    }
}
