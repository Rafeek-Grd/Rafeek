using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class _202606_AddCourseWeeklyHoursAndNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                schema: "dbo",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalPercent",
                schema: "dbo",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MidtermPercent",
                schema: "dbo",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectPercent",
                schema: "dbo",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyLabHours",
                schema: "dbo",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyLectureHours",
                schema: "dbo",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CourseId",
                schema: "dbo",
                table: "Notifications",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Courses_CourseId",
                schema: "dbo",
                table: "Notifications",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Courses_CourseId",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CourseId",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "FinalPercent",
                schema: "dbo",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MidtermPercent",
                schema: "dbo",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ProjectPercent",
                schema: "dbo",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "WeeklyLabHours",
                schema: "dbo",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "WeeklyLectureHours",
                schema: "dbo",
                table: "Courses");
        }
    }
}
