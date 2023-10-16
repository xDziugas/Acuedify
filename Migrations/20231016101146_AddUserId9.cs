using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_AcuedefyUserId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "AcuedefyUserId",
                table: "Quizzes",
                newName: "AcuedifyUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_AcuedefyUserId",
                table: "Quizzes",
                newName: "IX_Quizzes_AcuedifyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_AcuedifyUserId",
                table: "Quizzes",
                column: "AcuedifyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_AcuedifyUserId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "AcuedifyUserId",
                table: "Quizzes",
                newName: "AcuedefyUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_AcuedifyUserId",
                table: "Quizzes",
                newName: "IX_Quizzes_AcuedefyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_AcuedefyUserId",
                table: "Quizzes",
                column: "AcuedefyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
