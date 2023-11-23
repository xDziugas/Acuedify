using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_690 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ao3Score",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PastScoresSerialized",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesSolved",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ao3Score",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "PastScoresSerialized",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TimesSolved",
                table: "Quizzes");
        }
    }
}
