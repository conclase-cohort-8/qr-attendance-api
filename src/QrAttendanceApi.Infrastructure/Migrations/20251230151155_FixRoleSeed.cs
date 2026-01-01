using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrAttendanceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32751251-5533-4004-b248-d8d8ef427ce2",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:11:53 PM");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41de4677-6d68-4854-8262-cbeaa486fe4c",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:11:53 PM");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504ea7a6-fb72-43a8-8c1e-628bd4ababd1",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:11:53 PM");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef477b0-e65d-4c85-a6f3-8b44e89af17e",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:11:53 PM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32751251-5533-4004-b248-d8d8ef427ce2",
                column: "ConcurrencyStamp",
                value: "c1f0d0e0-0000-0000-0000-000000000001");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41de4677-6d68-4854-8262-cbeaa486fe4c",
                column: "ConcurrencyStamp",
                value: "c1f0d0e0-0000-0000-0000-000000000001");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504ea7a6-fb72-43a8-8c1e-628bd4ababd1",
                column: "ConcurrencyStamp",
                value: "c1f0d0e0-0000-0000-0000-000000000001");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef477b0-e65d-4c85-a6f3-8b44e89af17e",
                column: "ConcurrencyStamp",
                value: "c1f0d0e0-0000-0000-0000-000000000001");
        }
    }
}
