using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT Roles ON;
                IF NOT EXISTS(SELECT * FROM Roles WHERE RoleId = 1) INSERT INTO Roles (RoleId, Name) VALUES (1, 'Admin') ELSE UPDATE Roles SET Name = 'Admin' WHERE RoleId = 1;
                IF NOT EXISTS(SELECT * FROM Roles WHERE RoleId = 2) INSERT INTO Roles (RoleId, Name) VALUES (2, 'Shelf Manager') ELSE UPDATE Roles SET Name = 'Shelf Manager' WHERE RoleId = 2;
                IF NOT EXISTS(SELECT * FROM Roles WHERE RoleId = 3) INSERT INTO Roles (RoleId, Name) VALUES (3, 'Warehouse Manager') ELSE UPDATE Roles SET Name = 'Warehouse Manager' WHERE RoleId = 3;
                IF NOT EXISTS(SELECT * FROM Roles WHERE RoleId = 4) INSERT INTO Roles (RoleId, Name) VALUES (4, 'Sales Manager') ELSE UPDATE Roles SET Name = 'Sales Manager' WHERE RoleId = 4;
                SET IDENTITY_INSERT Roles OFF;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4);
        }
    }
}
