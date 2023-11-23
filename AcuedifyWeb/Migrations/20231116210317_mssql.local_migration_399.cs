using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_399 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ao3Score",
                table: "Quizzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ao3Score",
                table: "Quizzes",
                type: "int",
                nullable: true);
        }
    }
}
