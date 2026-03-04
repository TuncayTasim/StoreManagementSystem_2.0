using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyRejectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Rejections");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Rejections");

            migrationBuilder.DropColumn(
                name: "RejectionDate",
                table: "Rejections");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "Rejections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Rejections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Rejections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDate",
                table: "Rejections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "Rejections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
