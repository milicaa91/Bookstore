using BookCatalogService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishedAt { get; set; }

        public int StockQuantity { get; set; }
    }
}
