using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrAttendanceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "smart-attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32751251-5533-4004-b248-d8d8ef427ce2",
                column: "ConcurrencyStamp",
                value: "12/17/2025 5:44:06 PM");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41de4677-6d68-4854-8262-cbeaa486fe4c",
                column: "ConcurrencyStamp",
                value: "12/17/2025 5:44:06 PM");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504ea7a6-fb72-43a8-8c1e-628bd4ababd1",
                column: "ConcurrencyStamp",
                value: "12/17/2025 5:44:06 PM");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef477b0-e65d-4c85-a6f3-8b44e89af17e",
                column: "ConcurrencyStamp",
                value: "12/17/2025 5:44:06 PM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "smart-attendance");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32751251-5533-4004-b248-d8d8ef427ce2",
                column: "ConcurrencyStamp",
                value: "12/16/2025 5:24:30 PM");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41de4677-6d68-4854-8262-cbeaa486fe4c",
                column: "ConcurrencyStamp",
                value: "12/16/2025 5:24:30 PM");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504ea7a6-fb72-43a8-8c1e-628bd4ababd1",
                column: "ConcurrencyStamp",
                value: "12/16/2025 5:24:30 PM");

            migrationBuilder.UpdateData(
                schema: "smart-attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef477b0-e65d-4c85-a6f3-8b44e89af17e",
                column: "ConcurrencyStamp",
                value: "12/16/2025 5:24:30 PM");
        }
    }
}
