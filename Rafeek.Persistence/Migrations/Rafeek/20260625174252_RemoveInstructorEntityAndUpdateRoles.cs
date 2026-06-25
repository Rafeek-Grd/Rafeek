using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class RemoveInstructorEntityAndUpdateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Instructors_InstructorId",
                schema: "dbo",
                table: "Sections");

            // Add nullable DoctorId column before dropping Instructors so we can map existing data
            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                schema: "dbo",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            // Map existing Instructor references to the corresponding Doctor via shared UserId
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.[DoctorId] = d.[Id]
                FROM [dbo].[Sections] s
                INNER JOIN [dbo].[Instructors] i ON i.[Id] = s.[InstructorId]
                INNER JOIN [dbo].[Doctors] d ON d.[UserId] = i.[UserId]");

            // Drop the Instructors table and old InstructorId column
            migrationBuilder.DropTable(
                name: "Instructors",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Sections_InstructorId",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_DoctorId",
                schema: "dbo",
                table: "Sections",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Doctors_DoctorId",
                schema: "dbo",
                table: "Sections",
                column: "DoctorId",
                principalSchema: "dbo",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Doctors_DoctorId",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_DoctorId",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                schema: "dbo",
                table: "Sections");

            migrationBuilder.AddColumn<Guid>(
                name: "InstructorId",
                schema: "dbo",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Instructors",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instructors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "dbo",
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_InstructorId",
                schema: "dbo",
                table: "Sections",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_DepartmentId",
                schema: "dbo",
                table: "Instructors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                schema: "dbo",
                table: "Instructors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Instructors_InstructorId",
                schema: "dbo",
                table: "Sections",
                column: "InstructorId",
                principalSchema: "dbo",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
