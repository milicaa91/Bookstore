using Azure.Core;
using BookCatalogService.Application.Features.Books.Commands.AddBook;
using BookCatalogService.Application.Features.Books.Commands.DeleteBook;
using BookCatalogService.Application.Features.Books.Commands.UpdateBook;
using BookCatalogService.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace BookCatalogService.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly ISender _sender;
        public BookController(ISender sender)
        {
            _sender = sender;
        }

        //TODO GET /api/books?page=1&pageSize=10&searchFilter=authorName
        [HttpGet]
        public async Task<ActionResult> GetAllBooks([FromQuery] int pageIndex = 1,[FromQuery] int pageSize = 10, 
            [FromQuery] string searchFilter = null, CancellationToken cancellationToken = default )
        {
            if (pageIndex <= 0 || pageSize <= 0)
                return BadRequest("Page index and page size should be greater than zero!");

            var query = new GetAllBooksQuery(pageIndex, pageSize, searchFilter);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

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
