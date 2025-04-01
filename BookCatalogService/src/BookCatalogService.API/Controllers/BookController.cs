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
    /// <summary>
    /// Controller for managing books.
    /// </summary>
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


        /// <summary>
        /// Retrieves a list of books with optional filtering, sorting, and pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="title">The title to filter books by.</param>
        /// <param name="author">The author to filter books by.</param>
        /// <param name="category">The category to filter books by.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortOrder">The order to sort by (asc/desc).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of books.</returns>
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

        /// <summary>
        /// Retrieves the details of a specific book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The details of the book.</returns>
        [Authorize(Policy = "AdminOperatorUserPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseModel>> GetBookById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookDetailsQuery(id);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Adds a new book to the catalog.
        /// </summary>
        /// <param name="addBookRequest">The request containing the details of the book to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost]
        public async Task<ActionResult> AddBook([FromBody] AddBookRequest addBookRequest, CancellationToken cancellationToken)
        {
            var addBookCommand = new AddBookCommand(addBookRequest.Title, addBookRequest.Author, addBookRequest.Category,
                 addBookRequest.PublishedAt, addBookRequest.Price, addBookRequest.StockQuantity);

            var result = await _sender.Send(addBookCommand, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="updateBookRequest">The request containing the updated details of the book.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(Guid id, [FromBody] UpdateBookRequest updateBookRequest, 
            CancellationToken cancellationToken)
        {
            var updateBookCommand = new UpdateBookCommand(id, updateBookRequest.Title, updateBookRequest.Author, updateBookRequest.Category,
                 updateBookRequest.PublishedAt, updateBookRequest.Price, updateBookRequest.StockQuantity);

            var result = await _sender.Send(updateBookCommand, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a book from the catalog.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
        {
            var deleteBookCommand = new DeleteBookCommand(id);

            var result = await _sender.Send(deleteBookCommand, cancellationToken);
            return Ok(result);
        }
    }
}
