using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrAttendanceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "smart-attendance",
                newName: "RefreshTokens",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "smart-attendance",
                newName: "Departments",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "smart-attendance",
                newName: "AspNetUserTokens",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "smart-attendance",
                newName: "AspNetUsers",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "smart-attendance",
                newName: "AspNetUserRoles",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "smart-attendance",
                newName: "AspNetUserLogins",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "smart-attendance",
                newName: "AspNetUserClaims",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "smart-attendance",
                newName: "AspNetRoles",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "smart-attendance",
                newName: "AspNetRoleClaims",
                newSchema: "smart_attendance");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "smart_attendance",
                newName: "RefreshTokens",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "smart_attendance",
                newName: "Departments",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "smart_attendance",
                newName: "AspNetUserTokens",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "smart_attendance",
                newName: "AspNetUsers",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "smart_attendance",
                newName: "AspNetUserRoles",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "smart_attendance",
                newName: "AspNetUserLogins",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "smart_attendance",
                newName: "AspNetUserClaims",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "smart_attendance",
                newName: "AspNetRoles",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "smart_attendance",
                newName: "AspNetRoleClaims",
                newSchema: "smart-attendance");

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
    }
}
