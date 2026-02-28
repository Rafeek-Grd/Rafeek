using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class AddAcademicAdvisorRelationShipBetweenStudentAndDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicAdvisorId",
                schema: "dbo",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcademicAdvisor",
                schema: "dbo",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicAdvisorId",
                schema: "dbo",
                table: "Students",
                column: "AcademicAdvisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Doctors_AcademicAdvisorId",
                schema: "dbo",
                table: "Students",
                column: "AcademicAdvisorId",
                principalSchema: "dbo",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Doctors_AcademicAdvisorId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicAdvisorId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicAdvisorId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsAcademicAdvisor",
                schema: "dbo",
                table: "Doctors");
        }
    }
}
