using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class _202606_RenameSectionToLectureGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_Sections_SectionId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Sections_SectionId",
                schema: "dbo",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                schema: "dbo",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "Sections",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                schema: "dbo",
                table: "Enrollments",
                newName: "LectureGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_SectionId",
                schema: "dbo",
                table: "Enrollments",
                newName: "IX_Enrollments_LectureGroupId");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                schema: "dbo",
                table: "Assignment",
                newName: "LectureGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_SectionId",
                schema: "dbo",
                table: "Assignment",
                newName: "IX_Assignment_LectureGroupId");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                schema: "dbo",
                table: "AcademicCalendars",
                newName: "LectureGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicCalendars_SectionId",
                schema: "dbo",
                table: "AcademicCalendars",
                newName: "IX_AcademicCalendars_LectureGroupId");

            migrationBuilder.CreateTable(
                name: "LectureGroups",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LectureGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureGroups_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureGroups_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "dbo",
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LectureGroups_CourseId",
                schema: "dbo",
                table: "LectureGroups",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureGroups_DoctorId",
                schema: "dbo",
                table: "LectureGroups",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_LectureGroups_LectureGroupId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "LectureGroupId",
                principalSchema: "dbo",
                principalTable: "LectureGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_LectureGroups_LectureGroupId",
                schema: "dbo",
                table: "Assignment",
                column: "LectureGroupId",
                principalSchema: "dbo",
                principalTable: "LectureGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_LectureGroups_LectureGroupId",
                schema: "dbo",
                table: "Enrollments",
                column: "LectureGroupId",
                principalSchema: "dbo",
                principalTable: "LectureGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendars_LectureGroups_LectureGroupId",
                schema: "dbo",
                table: "AcademicCalendars");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_LectureGroups_LectureGroupId",
                schema: "dbo",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_LectureGroups_LectureGroupId",
                schema: "dbo",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "LectureGroups",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "LectureGroupId",
                schema: "dbo",
                table: "Enrollments",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_LectureGroupId",
                schema: "dbo",
                table: "Enrollments",
                newName: "IX_Enrollments_SectionId");

            migrationBuilder.RenameColumn(
                name: "LectureGroupId",
                schema: "dbo",
                table: "Assignment",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_LectureGroupId",
                schema: "dbo",
                table: "Assignment",
                newName: "IX_Assignment_SectionId");

            migrationBuilder.RenameColumn(
                name: "LectureGroupId",
                schema: "dbo",
                table: "AcademicCalendars",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicCalendars_LectureGroupId",
                schema: "dbo",
                table: "AcademicCalendars",
                newName: "IX_AcademicCalendars_SectionId");

            migrationBuilder.CreateTable(
                name: "Sections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "dbo",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "dbo",
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CourseId",
                schema: "dbo",
                table: "Sections",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_DoctorId",
                schema: "dbo",
                table: "Sections",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendars_Sections_SectionId",
                schema: "dbo",
                table: "AcademicCalendars",
                column: "SectionId",
                principalSchema: "dbo",
                principalTable: "Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Sections_SectionId",
                schema: "dbo",
                table: "Assignment",
                column: "SectionId",
                principalSchema: "dbo",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                schema: "dbo",
                table: "Enrollments",
                column: "SectionId",
                principalSchema: "dbo",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
