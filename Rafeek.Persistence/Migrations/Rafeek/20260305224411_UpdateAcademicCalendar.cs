using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class UpdateAcademicCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsAllDay",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "IsAllDay",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "dbo",
                table: "AcademicCalendars");
        }
    }
}
