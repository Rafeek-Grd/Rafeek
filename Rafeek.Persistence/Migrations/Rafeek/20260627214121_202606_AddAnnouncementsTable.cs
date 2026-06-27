using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class _202606_AddAnnouncementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudienceType = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetLevel = table.Column<int>(type: "int", nullable: true),
                    SendInApp = table.Column<bool>(type: "bit", nullable: false),
                    SendEmail = table.Column<bool>(type: "bit", nullable: false),
                    SendSMS = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeactivated = table.Column<bool>(type: "bit", nullable: false),
                    PostponedTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "dbo",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_DepartmentId",
                schema: "dbo",
                table: "Announcements",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ScheduledAt",
                schema: "dbo",
                table: "Announcements",
                column: "ScheduledAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements",
                schema: "dbo");
        }
    }
}
