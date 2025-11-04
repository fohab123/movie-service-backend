using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Series");

            migrationBuilder.RenameColumn(
                name: "Episodes",
                table: "Series",
                newName: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_GenreId",
                table: "Series",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Genres_GenreId",
                table: "Series",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Genres_GenreId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_GenreId",
                table: "Series");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Series",
                newName: "Episodes");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Series",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
