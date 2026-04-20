using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChatSessionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                schema: "dbo",
                table: "ChatbotQueries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ChatSessions", x => x.Id);
                });

            // Insert a dummy chat session to satisfy foreign key constraints for existing records
            migrationBuilder.Sql("INSERT INTO [dbo].[ChatSessions] ([Id], [UserId], [Title], [CreatedAt], [CreatedBy], [IsActive], [IsDeleted]) VALUES ('00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', N'محادثة سابقة', GETDATE(), N'System', 1, 0)");

            migrationBuilder.CreateIndex(
                name: "IX_ChatbotQueries_SessionId",
                schema: "dbo",
                table: "ChatbotQueries",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatbotQueries_ChatSessions_SessionId",
                schema: "dbo",
                table: "ChatbotQueries",
                column: "SessionId",
                principalSchema: "dbo",
                principalTable: "ChatSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatbotQueries_ChatSessions_SessionId",
                schema: "dbo",
                table: "ChatbotQueries");

            migrationBuilder.DropTable(
                name: "ChatSessions",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_ChatbotQueries_SessionId",
                schema: "dbo",
                table: "ChatbotQueries");

            migrationBuilder.DropColumn(
                name: "SessionId",
                schema: "dbo",
                table: "ChatbotQueries");
        }
    }
}
