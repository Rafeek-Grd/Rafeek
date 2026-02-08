using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class SolveConflictOfPrerequisteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePrerequisites",
                schema: "dbo",
                table: "CoursePrerequisites");

            migrationBuilder.DropIndex(
                name: "IX_CoursePrerequisites_CourseId",
                schema: "dbo",
                table: "CoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "CoursePrerequisites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePrerequisites",
                schema: "dbo",
                table: "CoursePrerequisites",
                columns: new[] { "CourseId", "PrerequisiteId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CoursePrerequisites",
                schema: "dbo",
                table: "CoursePrerequisites");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "CoursePrerequisites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoursePrerequisites",
                schema: "dbo",
                table: "CoursePrerequisites",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePrerequisites_CourseId",
                schema: "dbo",
                table: "CoursePrerequisites",
                column: "CourseId");
        }
    }
}
