using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafeek.Persistence.Migrations.Rafeek
{
    public partial class AddAcademicAdvisorRelationShipBetweenStudentAndDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicAdvisorId",
                schema: "dbo",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id 
                           WHERE s.name = 'dbo' AND t.name = 'Doctors')
                BEGIN
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Doctors') AND name = 'IsAcademicAdvisor')
                    BEGIN
                        ALTER TABLE [dbo].[Doctors] ADD [IsAcademicAdvisor] bit NOT NULL DEFAULT 0;
                    END
                END
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicAdvisorId",
                schema: "dbo",
                table: "Students",
                column: "AcademicAdvisorId");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id 
                           WHERE s.name = 'dbo' AND t.name = 'Doctors')
                BEGIN
                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Students_Doctors_AcademicAdvisorId')
                    BEGIN
                        ALTER TABLE [dbo].[Students] ADD CONSTRAINT [FK_Students_Doctors_AcademicAdvisorId] 
                        FOREIGN KEY ([AcademicAdvisorId]) REFERENCES [dbo].[Doctors] ([Id]);
                    END
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Students_Doctors_AcademicAdvisorId')
                BEGIN
                    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Doctors_AcademicAdvisorId];
                END
            ");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicAdvisorId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicAdvisorId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id 
                           WHERE s.name = 'dbo' AND t.name = 'Doctors')
                BEGIN
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Doctors') AND name = 'IsAcademicAdvisor')
                    BEGIN
                        ALTER TABLE [dbo].[Doctors] DROP COLUMN [IsAcademicAdvisor];
                    END
                END
            ");
        }
    }
}
