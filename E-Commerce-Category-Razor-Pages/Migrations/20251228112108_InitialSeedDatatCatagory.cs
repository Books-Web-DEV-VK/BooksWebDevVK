using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksWeb_Category_Razor_Pages.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedDatatCatagory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { new Guid("05a26538-8bb2-4453-859c-ceff1b709aa2"), 5, "Spiritual" },
                    { new Guid("3d9e9ca2-1896-4545-ad23-c50be86eb8b8"), 2, "Suspense Thriller" },
                    { new Guid("43dfc162-5fa0-4082-a33c-b492e9e273e1"), 4, "Horror" },
                    { new Guid("883ce9a3-dc32-484d-8936-eeac4979f368"), 1, "Action" },
                    { new Guid("fe20e0e2-0664-4e47-978c-8a829672fc1f"), 3, "Romance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
