using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class updateAcademinCalendarTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventType",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetUserId",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendars_TargetUserId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "TargetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_ApplicationUsers_TargetUserId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "TargetUserId",
                principalSchema: "auth",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_ApplicationUsers_TargetUserId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendars_TargetUserId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "EventType",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "TargetUserId",
                schema: "dbo",
                table: "AcademicCalendars");
        }
    }
}
