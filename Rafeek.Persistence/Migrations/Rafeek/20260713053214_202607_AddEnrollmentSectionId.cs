using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class _202607_AddEnrollmentSectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                schema: "dbo",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SectionId",
                schema: "dbo",
                table: "Enrollments",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_CourseSections_SectionId",
                schema: "dbo",
                table: "Enrollments",
                column: "SectionId",
                principalSchema: "dbo",
                principalTable: "CourseSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_CourseSections_SectionId",
                schema: "dbo",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_SectionId",
                schema: "dbo",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "dbo",
                table: "Enrollments");
        }
    }
}
