using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcuedefyUserId",
                table: "Quizzes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AcuedefyUserId",
                table: "Quizzes",
                column: "AcuedefyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_AcuedefyUserId",
                table: "Quizzes",
                column: "AcuedefyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_AcuedefyUserId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_AcuedefyUserId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "AcuedefyUserId",
                table: "Quizzes");
        }
    }
}
