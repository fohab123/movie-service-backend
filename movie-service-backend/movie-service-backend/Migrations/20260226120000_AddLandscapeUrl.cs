using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLandscapeUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LandscapeUrl",
                table: "Films",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandscapeUrl",
                table: "Series",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandscapeUrl",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "LandscapeUrl",
                table: "Series");
        }
    }
}
