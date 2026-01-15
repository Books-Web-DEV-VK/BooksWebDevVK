using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductTable_With_SeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("13e0d328-d995-4293-ba1b-9a479646f77e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3351141f-420a-442a-97ca-c80b881e9f9e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("68e0dcef-7798-4ee2-8b78-94a77ff56ea8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("82fe0a0e-4e5b-4f24-b686-c125cc85ba7e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e7f2ccb8-4ce5-4118-a458-94da45a34f02"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { new Guid("13e0d328-d995-4293-ba1b-9a479646f77e"), 5, "Spiritual" },
                    { new Guid("3351141f-420a-442a-97ca-c80b881e9f9e"), 1, "Action" },
                    { new Guid("68e0dcef-7798-4ee2-8b78-94a77ff56ea8"), 2, "Suspense Thriller" },
                    { new Guid("82fe0a0e-4e5b-4f24-b686-c125cc85ba7e"), 3, "Romance" },
                    { new Guid("e7f2ccb8-4ce5-4118-a458-94da45a34f02"), 4, "Horror" }
                });
        }
    }
}
