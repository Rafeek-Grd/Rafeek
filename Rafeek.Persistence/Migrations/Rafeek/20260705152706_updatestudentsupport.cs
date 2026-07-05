using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class updatestudentsupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSupports_Students_StudentId",
                schema: "dbo",
                table: "StudentSupports");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "dbo",
                table: "StudentSupports",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "StudentSupports",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketType",
                schema: "dbo",
                table: "StudentSupports",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSupports_Students_StudentId",
                schema: "dbo",
                table: "StudentSupports",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSupports_Students_StudentId",
                schema: "dbo",
                table: "StudentSupports");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "StudentSupports");

            migrationBuilder.DropColumn(
                name: "TicketType",
                schema: "dbo",
                table: "StudentSupports");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "dbo",
                table: "StudentSupports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSupports_Students_StudentId",
                schema: "dbo",
                table: "StudentSupports",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
