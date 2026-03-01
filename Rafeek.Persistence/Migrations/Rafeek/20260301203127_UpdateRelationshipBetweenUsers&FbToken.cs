using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class UpdateRelationshipBetweenUsersFbToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFbTokens_Doctors_DoctorId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFbTokens_Instructors_InstructorId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFbTokens_Students_StudentId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserFbTokens_DoctorId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserFbTokens_InstructorId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserFbTokens_StudentId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropColumn(
                name: "StudentId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.CreateIndex(
                name: "IX_UserFbTokens_UserId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFbTokens_ApplicationUsers_UserId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "UserId",
                principalSchema: "auth",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFbTokens_ApplicationUsers_UserId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserFbTokens_UserId",
                schema: "dbo",
                table: "UserFbTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                schema: "dbo",
                table: "UserFbTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstructorId",
                schema: "dbo",
                table: "UserFbTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                schema: "dbo",
                table: "UserFbTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFbTokens_DoctorId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFbTokens_InstructorId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFbTokens_StudentId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFbTokens_Doctors_DoctorId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "DoctorId",
                principalSchema: "dbo",
                principalTable: "Doctors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFbTokens_Instructors_InstructorId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "InstructorId",
                principalSchema: "dbo",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFbTokens_Students_StudentId",
                schema: "dbo",
                table: "UserFbTokens",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
