using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmocineSveikataServer.Migrations
{
    /// <inheritdoc />
    public partial class NavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpecialistProfiles_UserId",
                table: "SpecialistProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialistProfiles_UserId",
                table: "SpecialistProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpecialistProfiles_UserId",
                table: "SpecialistProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialistProfiles_UserId",
                table: "SpecialistProfiles",
                column: "UserId");
        }
    }
}
