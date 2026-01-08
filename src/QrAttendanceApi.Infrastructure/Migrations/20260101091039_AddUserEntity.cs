using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrAttendanceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "QrSessions",
                schema: "smart-attendance",
                newName: "QrSessions",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                schema: "smart-attendance",
                newName: "AuditLogs",
                newSchema: "smart_attendance");

            migrationBuilder.RenameTable(
                name: "AttendanceLogs",
                schema: "smart-attendance",
                newName: "AttendanceLogs",
                newSchema: "smart_attendance");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32751251-5533-4004-b248-d8d8ef427ce2",
                column: "ConcurrencyStamp",
                value: "STAFF_ROLE");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41de4677-6d68-4854-8262-cbeaa486fe4c",
                column: "ConcurrencyStamp",
                value: "SUPERADMIN_ROLE");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504ea7a6-fb72-43a8-8c1e-628bd4ababd1",
                column: "ConcurrencyStamp",
                value: "ADMIN_ROLE");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef477b0-e65d-4c85-a6f3-8b44e89af17e",
                column: "ConcurrencyStamp",
                value: "STUDENT_ROLE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "QrSessions",
                schema: "smart_attendance",
                newName: "QrSessions",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                schema: "smart_attendance",
                newName: "AuditLogs",
                newSchema: "smart-attendance");

            migrationBuilder.RenameTable(
                name: "AttendanceLogs",
                schema: "smart_attendance",
                newName: "AttendanceLogs",
                newSchema: "smart-attendance");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32751251-5533-4004-b248-d8d8ef427ce2",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:43:49 AM");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41de4677-6d68-4854-8262-cbeaa486fe4c",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:43:49 AM");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504ea7a6-fb72-43a8-8c1e-628bd4ababd1",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:43:49 AM");

            migrationBuilder.UpdateData(
                schema: "smart_attendance",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bef477b0-e65d-4c85-a6f3-8b44e89af17e",
                column: "ConcurrencyStamp",
                value: "12/30/2025 3:43:49 AM");
        }
    }
}
