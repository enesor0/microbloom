using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace microbloom.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2025, 12, 22, 22, 51, 48, 722, DateTimeKind.Utc).AddTicks(934));

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2025, 12, 22, 22, 51, 48, 722, DateTimeKind.Utc).AddTicks(938));

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 3,
                column: "PostedDate",
                value: new DateTime(2025, 12, 22, 22, 51, 48, 722, DateTimeKind.Utc).AddTicks(939));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2025, 12, 22, 22, 2, 33, 272, DateTimeKind.Utc).AddTicks(2751));

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2025, 12, 22, 22, 2, 33, 272, DateTimeKind.Utc).AddTicks(2754));

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 3,
                column: "PostedDate",
                value: new DateTime(2025, 12, 22, 22, 2, 33, 272, DateTimeKind.Utc).AddTicks(2755));
        }
    }
}
