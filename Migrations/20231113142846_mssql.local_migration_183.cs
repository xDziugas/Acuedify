using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_183 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPlayed",
                table: "Quizzes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPlayed",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Question");
        }
    }
}
