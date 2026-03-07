using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class updateAcademicCalendarTblWithOtherReferencedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                schema: "dbo",
                table: "Sections",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                schema: "dbo",
                table: "Sections",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                schema: "dbo",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                schema: "dbo",
                table: "Appointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "dbo",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "dbo",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                schema: "dbo",
                table: "Appointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicTermId",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurrenceEndDate",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceType",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                schema: "dbo",
                table: "AcademicCalendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AcademicYears",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCalendarPreferences",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowAcademicEvents = table.Column<bool>(type: "bit", nullable: false),
                    ShowGuidanceEvents = table.Column<bool>(type: "bit", nullable: false),
                    ShowDeadlines = table.Column<bool>(type: "bit", nullable: false),
                    ShowOfficialHolidays = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCalendarPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCalendarPreferences_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicTerms",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DropDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicTerms_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalSchema: "dbo",
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendars_AcademicTermId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "AcademicTermId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendars_CourseId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendars_DepartmentId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendars_SectionId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicTerms_AcademicYearId",
                schema: "dbo",
                table: "AcademicTerms",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendarPreferences_UserId",
                schema: "dbo",
                table: "UserCalendarPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_AcademicTerms_AcademicTermId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "AcademicTermId",
                principalSchema: "dbo",
                principalTable: "AcademicTerms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_Courses_CourseId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "CourseId",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_Departments_DepartmentId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "DepartmentId",
                principalSchema: "dbo",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_Sections_SectionId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "SectionId",
                principalSchema: "dbo",
                principalTable: "Sections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_AcademicTerms_AcademicTermId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_Courses_CourseId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_Departments_DepartmentId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_Sections_SectionId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropTable(
                name: "AcademicTerms",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserCalendarPreferences",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AcademicYears",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendars_AcademicTermId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendars_CourseId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendars_DepartmentId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendars_SectionId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                schema: "dbo",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "dbo",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "dbo",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "dbo",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "dbo",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AcademicTermId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "RecurrenceEndDate",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "RecurrenceType",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropColumn(
                name: "Visibility",
                schema: "dbo",
                table: "AcademicCalendars");
        }
    }
}
