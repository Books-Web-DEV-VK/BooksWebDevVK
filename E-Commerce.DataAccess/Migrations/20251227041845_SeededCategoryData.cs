using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeededCategoryData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
