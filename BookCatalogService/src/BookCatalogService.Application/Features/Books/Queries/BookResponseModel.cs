using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Queries
{
    public record BookResponseModel
    {
        public BookResponseModel(Guid id, string title, string author, decimal price, DateTime publishedAt, int stockQuantity)
        {
            Id = id;
            Title = title;
            Author = author;
            Price = price;
            PublishedAt = publishedAt;
            StockQuantity = stockQuantity;
        }

        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Author { get; init; }
        public decimal Price { get; init; }
        //public string Genre { get; init; } //TODO MN
        public DateTime PublishedAt { get; init; }
        public int StockQuantity { get; init; }
    }
}
