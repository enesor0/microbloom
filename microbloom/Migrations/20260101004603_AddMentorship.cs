using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace microbloom.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileDownloadUrl",
                table: "CvSamples",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "MentorshipApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenteeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorshipApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorshipApplications_AspNetUsers_MenteeId",
                        column: x => x.MenteeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorshipApplications_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 1,
                column: "PostedDate",
                value: new DateTime(2026, 1, 1, 0, 46, 2, 721, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 2,
                column: "PostedDate",
                value: new DateTime(2026, 1, 1, 0, 46, 2, 721, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "JobPostings",
                keyColumn: "Id",
                keyValue: 3,
                column: "PostedDate",
                value: new DateTime(2026, 1, 1, 0, 46, 2, 721, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipApplications_MenteeId",
                table: "MentorshipApplications",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipApplications_MentorId",
                table: "MentorshipApplications",
                column: "MentorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorshipApplications");

            migrationBuilder.AlterColumn<string>(
                name: "FileDownloadUrl",
                table: "CvSamples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
