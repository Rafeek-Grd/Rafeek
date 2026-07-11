using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId1",
                schema: "dbo",
                table: "CourseSections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSections_CourseId1",
                schema: "dbo",
                table: "CourseSections",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSections_Courses_CourseId1",
                schema: "dbo",
                table: "CourseSections",
                column: "CourseId1",
                principalSchema: "dbo",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSections_Courses_CourseId1",
                schema: "dbo",
                table: "CourseSections");

            migrationBuilder.DropIndex(
                name: "IX_CourseSections_CourseId1",
                schema: "dbo",
                table: "CourseSections");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                schema: "dbo",
                table: "CourseSections");
        }
    }
}
