using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acuedify.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToFolderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Folders");
        }
    }
}
