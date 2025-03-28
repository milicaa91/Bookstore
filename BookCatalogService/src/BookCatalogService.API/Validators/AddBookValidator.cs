using BookCatalogService.Application.Features.Books.Commands.AddBook;
using BookCatalogService.Domain.Entities;
using FluentValidation;

namespace BookCatalogService.API.Validators
{
    public class AddBookValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookValidator()
        {
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

            RuleFor(book => book.Author)
                .NotEmpty().WithMessage("Author is required.")
                .Length(3, 100).WithMessage("Author must be between 3 and 100 characters.");

            RuleFor(book => book.Category)
                .IsInEnum().WithMessage("Invalid category.");

            RuleFor(book => book.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(book => book.PublishedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Published date must be in the past or present.");

            RuleFor(book => book.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
        }
    }
}
