using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmocineSveikataServer.Migrations
{
    /// <inheritdoc />
    public partial class LikeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "LikedBy",
                table: "Discussions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "LikedBy",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedBy",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "LikedBy",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Discussions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
