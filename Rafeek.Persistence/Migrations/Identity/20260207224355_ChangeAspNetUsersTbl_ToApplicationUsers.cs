using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAspNetUsersTbl_ToApplicationUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "auth",
                newName: "ApplicationUsers",
                newSchema: "auth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                schema: "auth",
                newName: "AspNetUsers",
                newSchema: "auth");
        }
    }
}
