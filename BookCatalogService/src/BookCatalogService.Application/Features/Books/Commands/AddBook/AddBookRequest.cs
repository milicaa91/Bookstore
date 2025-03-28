using BookCatalogService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.AddBook
{
    public class AddBookRequest
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishedAt { get; set; }

        public int StockQuantity { get; set; }
    }
}
