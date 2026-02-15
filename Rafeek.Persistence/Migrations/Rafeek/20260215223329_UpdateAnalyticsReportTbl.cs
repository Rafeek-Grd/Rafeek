using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class UpdateAnalyticsReportTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnalyticsReports_GeneratedAt",
                schema: "dbo",
                table: "AnalyticsReports");

            migrationBuilder.DropIndex(
                name: "IX_AnalyticsReports_ReportType",
                schema: "dbo",
                table: "AnalyticsReports");

            migrationBuilder.DropColumn(
                name: "GeneratedAt",
                schema: "dbo",
                table: "AnalyticsReports");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                schema: "dbo",
                table: "AnalyticsReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                schema: "dbo",
                table: "AnalyticsReports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneratedAt",
                schema: "dbo",
                table: "AnalyticsReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsReports_GeneratedAt",
                schema: "dbo",
                table: "AnalyticsReports",
                column: "GeneratedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsReports_ReportType",
                schema: "dbo",
                table: "AnalyticsReports",
                column: "ReportType");
        }
    }
}
