using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "Role" },
                values: new object[] { 6, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "employee@dosthi.com", "Employee", "$2a$11$N0NMPg7x.c2cEfyZGyb72.fWUe3esmtZl0j49DH1UeV9e7CCS0o.C", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
