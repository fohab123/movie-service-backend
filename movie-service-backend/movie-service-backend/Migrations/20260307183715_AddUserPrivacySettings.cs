using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPrivacySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentsVisibility",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HideEmail",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PersonalisedRecs",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RatingsVisibility",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WatchlistVisibility",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HideEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonalisedRecs",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RatingsVisibility",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WatchlistVisibility",
                table: "Users");
        }
    }
}
