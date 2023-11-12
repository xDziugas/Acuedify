using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ao3Score",
                table: "Quizzes",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ao3Score",
                table: "Quizzes");
        }
    }
}
