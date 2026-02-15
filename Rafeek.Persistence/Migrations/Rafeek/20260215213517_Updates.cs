using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Students_StudentId",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SendTime",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Type",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SendTime",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetGroup",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "dbo",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_StudentId",
                schema: "dbo",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "dbo",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                schema: "dbo",
                table: "Notifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ApplicationUsers_UserId",
                schema: "dbo",
                table: "Notifications",
                column: "UserId",
                principalSchema: "auth",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ApplicationUsers_UserId",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "Notifications",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                schema: "dbo",
                table: "Notifications",
                newName: "IX_Notifications_StudentId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "dbo",
                table: "Notifications",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                schema: "dbo",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendTime",
                schema: "dbo",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetGroup",
                schema: "dbo",
                table: "Notifications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SendTime",
                schema: "dbo",
                table: "Notifications",
                column: "SendTime");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Type",
                schema: "dbo",
                table: "Notifications",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Students_StudentId",
                schema: "dbo",
                table: "Notifications",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
