using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmocineSveikataServer.Migrations
{
    /// <inheritdoc />
    public partial class MeetLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetLink",
                table: "SpecialistTimeSlots",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetLink",
                table: "SpecialistTimeSlots");
        }
    }
}
