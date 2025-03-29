using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Queries
{
    public class BookResponseModel
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

        [JsonProperty("Id")]
        public Guid Id { get; init; }

        [JsonProperty("Title")]
        public string Title { get; init; }

        [JsonProperty("Author")]
        public string Author { get; init; }

        [JsonProperty("Price")]
        public decimal Price { get; init; }
        //public string Genre { get; init; } //TODO MN
        [JsonProperty("PublishedAt")]
        public DateTime PublishedAt { get; init; } //TODO MN converter

        [JsonProperty("StockQuantity")]
        public int StockQuantity { get; init; }
    }
}
