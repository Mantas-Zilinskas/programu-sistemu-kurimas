using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmocineSveikataServer.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionOnDiscussions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Discussions",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Discussions");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");
        }
    }
}
