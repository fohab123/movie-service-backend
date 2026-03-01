using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDebatePostFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilmId",
                table: "DebatePosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpoiler",
                table: "DebatePosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "DebatePosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "DebatePosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "DebatePosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DebatePosts_FilmId",
                table: "DebatePosts",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_DebatePosts_SeriesId",
                table: "DebatePosts",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DebatePosts_Films_FilmId",
                table: "DebatePosts",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebatePosts_Series_SeriesId",
                table: "DebatePosts",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebatePosts_Films_FilmId",
                table: "DebatePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DebatePosts_Series_SeriesId",
                table: "DebatePosts");

            migrationBuilder.DropIndex(
                name: "IX_DebatePosts_FilmId",
                table: "DebatePosts");

            migrationBuilder.DropIndex(
                name: "IX_DebatePosts_SeriesId",
                table: "DebatePosts");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "DebatePosts");

            migrationBuilder.DropColumn(
                name: "IsSpoiler",
                table: "DebatePosts");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "DebatePosts");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "DebatePosts");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "DebatePosts");
        }
    }
}
