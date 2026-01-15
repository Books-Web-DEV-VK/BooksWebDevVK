using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationForeignKeyForProductsToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3b57ff31-5e99-4fa7-b691-f87b7588f52d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4a2e3b38-1795-460e-ac53-bf566ba13808"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4e3aef30-4a4a-4669-951f-494cd788a563"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("addadd01-0f08-4b7d-8725-a10b5ba86ad1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bd57289d-fa54-4aed-a3ff-206df05c6991"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("37065aca-1f10-4555-8759-5c42d8f374f7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("38dcf8d0-4aec-4f5a-85b6-bd285406c24d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9a5d79dd-3b0c-4f52-87f1-5bcbc3abb704"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ceee96ba-c00f-4186-99ee-e4291bb6fef9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e91fc6ee-e39a-4d88-b4b2-8dffd14b83d4"));

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { new Guid("60592bbc-bc78-4b61-bfb0-925b3a930c23"), 1, "Action" },
                    { new Guid("7b3c5d6e-7f8a-4b9c-0d1e-2f3a4b5c6d7e"), 4, "Horror" },
                    { new Guid("8f1a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c"), 2, "Suspense Thriller" },
                    { new Guid("9a2b4c5d-6e7f-4a8b-9c0d-1e2f3a4b5c6d"), 5, "Spiritual" },
                    { new Guid("ca7b4215-d1ac-4091-a194-2ffee85ed84f"), 3, "Romance" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), "Billy Spark", new Guid("60592bbc-bc78-4b61-bfb0-925b3a930c23"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "SWD9999001", 99.0, 90.0, 80.0, 85.0, "Fortune of Time" },
                    { new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"), "Nancy Hoover", new Guid("60592bbc-bc78-4b61-bfb0-925b3a930c23"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "CAW7777001", 40.0, 30.0, 20.0, 25.0, "Dark Skies" },
                    { new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), "Julian Button", new Guid("ca7b4215-d1ac-4091-a194-2ffee85ed84f"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "RITO5555001", 55.0, 50.0, 35.0, 40.0, "Vanish in the Sunset" },
                    { new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), "Abby Muscles", new Guid("8f1a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "WS3333001", 70.0, 65.0, 55.0, 60.0, "Cotton Candy" },
                    { new Guid("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), "Ron Parker", new Guid("9a2b4c5d-6e7f-4a8b-9c0d-1e2f3a4b5c6d"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "SOTJ1111001", 30.0, 27.0, 20.0, 25.0, "Rock in the Ocean" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7b3c5d6e-7f8a-4b9c-0d1e-2f3a4b5c6d7e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("60592bbc-bc78-4b61-bfb0-925b3a930c23"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8f1a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9a2b4c5d-6e7f-4a8b-9c0d-1e2f3a4b5c6d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ca7b4215-d1ac-4091-a194-2ffee85ed84f"));

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { new Guid("3b57ff31-5e99-4fa7-b691-f87b7588f52d"), 5, "Spiritual" },
                    { new Guid("4a2e3b38-1795-460e-ac53-bf566ba13808"), 1, "Action" },
                    { new Guid("4e3aef30-4a4a-4669-951f-494cd788a563"), 3, "Romance" },
                    { new Guid("addadd01-0f08-4b7d-8725-a10b5ba86ad1"), 4, "Horror" },
                    { new Guid("bd57289d-fa54-4aed-a3ff-206df05c6991"), 2, "Suspense Thriller" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { new Guid("37065aca-1f10-4555-8759-5c42d8f374f7"), "Nancy Hoover", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "CAW7777001", 40.0, 30.0, 20.0, 25.0, "Dark Skies" },
                    { new Guid("38dcf8d0-4aec-4f5a-85b6-bd285406c24d"), "Julian Button", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "RITO5555001", 55.0, 50.0, 35.0, 40.0, "Vanish in the Sunset" },
                    { new Guid("9a5d79dd-3b0c-4f52-87f1-5bcbc3abb704"), "Billy Spark", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "SWD9999001", 99.0, 90.0, 80.0, 85.0, "Fortune of Time" },
                    { new Guid("ceee96ba-c00f-4186-99ee-e4291bb6fef9"), "Abby Muscles", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "WS3333001", 70.0, 65.0, 55.0, 60.0, "Cotton Candy" },
                    { new Guid("e91fc6ee-e39a-4d88-b4b2-8dffd14b83d4"), "Ron Parker", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.", "SOTJ1111001", 30.0, 27.0, 20.0, 25.0, "Rock in the Ocean" }
                });
        }
    }
}
