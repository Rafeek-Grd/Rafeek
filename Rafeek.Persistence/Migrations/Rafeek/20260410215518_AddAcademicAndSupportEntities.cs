using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    /// <inheritdoc />
    public partial class AddAcademicAndSupportEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "AcademicYears");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentYear",
                schema: "dbo",
                table: "AcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrentYear",
                schema: "dbo",
                table: "AcademicYears");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "AcademicYears",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
