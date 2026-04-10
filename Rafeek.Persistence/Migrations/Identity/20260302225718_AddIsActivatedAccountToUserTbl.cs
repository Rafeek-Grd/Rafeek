using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddIsActivatedAccountToUserTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUniversityEmailActivated",
                schema: "auth",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUniversityEmailActivated",
                schema: "auth",
                table: "ApplicationUsers");
        }
    }
}
