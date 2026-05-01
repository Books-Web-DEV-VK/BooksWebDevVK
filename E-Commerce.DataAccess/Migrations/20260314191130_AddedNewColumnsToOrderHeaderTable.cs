using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumnsToOrderHeaderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefundId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
