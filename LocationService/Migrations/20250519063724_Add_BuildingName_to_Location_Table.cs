using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationService.Migrations
{
    /// <inheritdoc />
    public partial class Add_BuildingName_to_Location_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildingName",
                table: "Location",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingName",
                table: "Location");
        }
    }
}
