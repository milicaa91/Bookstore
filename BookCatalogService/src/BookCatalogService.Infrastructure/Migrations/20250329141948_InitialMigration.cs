using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookCatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Category", "Price", "PublishedAt", "StockQuantity", "Title" },
                values: new object[,]
                {
                    { new Guid("1182443b-6bc3-400e-a9f6-ff74631b4ee5"), "Robert C. Martin", 1, 19.99m, new DateTime(2008, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Clean Code" },
                    { new Guid("1d74449c-c3b9-4452-b08c-10bbe67decbc"), "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", 1, 42.99m, new DateTime(1994, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Design Patterns" },
                    { new Guid("4731710e-f850-41ea-b784-060990c98c0c"), "George R.R. Martin", 2, 29.99m, new DateTime(1996, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, "A Game of Thrones" },
                    { new Guid("642f8f31-f836-4035-97c2-6c7e9aeef8dc"), "Gillian Flynn", 8, 14.99m, new DateTime(1996, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, "Gone Girl" },
                    { new Guid("67511f83-662a-4ec7-9d34-1e1ebec09b4c"), "", 8, 14.99m, new DateTime(1999, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, "The Silence of the Lambs" },
                    { new Guid("6bb27932-6f2b-4353-a1b8-abbf34e06d96"), "Harper Lee", 13, 15.99m, new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "To Kill a Mockingbird" },
                    { new Guid("83972487-6423-4d1b-8c76-2035646fba6d"), "F. Scott Fitzgerald", 13, 13.50m, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "The Great Gatsby" },
                    { new Guid("e07821e7-309d-4c8e-b288-531332f59b8f"), "Fyodor Dostoevsky", 12, 17.99m, new DateTime(1866, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Crime and Punishment" },
                    { new Guid("ee7d491f-a0dc-4f55-b959-252db34de20f"), "Mario Puzo", 13, 21.99m, new DateTime(1969, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "The Godfather" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Author",
                table: "Books",
                column: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Category",
                table: "Books",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Title",
                table: "Books",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Title_Author_Category",
                table: "Books",
                columns: new[] { "Title", "Author", "Category" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
