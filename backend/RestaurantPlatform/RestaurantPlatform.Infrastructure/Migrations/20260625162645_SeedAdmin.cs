using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "Role" },
                values: new object[] { 2, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "$2a$11$CUx5HnTlYDICDSeOfavhXOU2YXdhc9onAEO1X5Ba.gakC84GJ.it.", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
