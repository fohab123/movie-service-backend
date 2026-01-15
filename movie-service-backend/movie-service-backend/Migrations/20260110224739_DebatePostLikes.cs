using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_service_backend.Migrations
{
    /// <inheritdoc />
    public partial class DebatePostLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings");

            migrationBuilder.CreateTable(
                name: "DebatePostLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DebatePostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebatePostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebatePostLikes_DebatePosts_DebatePostId",
                        column: x => x.DebatePostId,
                        principalTable: "DebatePosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DebatePostLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_FilmId",
                table: "Ratings",
                columns: new[] { "UserId", "FilmId" },
                unique: true,
                filter: "[FilmId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_SeriesId",
                table: "Ratings",
                columns: new[] { "UserId", "SeriesId" },
                unique: true,
                filter: "[SeriesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DebatePostLikes_DebatePostId",
                table: "DebatePostLikes",
                column: "DebatePostId");

            migrationBuilder.CreateIndex(
                name: "IX_DebatePostLikes_UserId_DebatePostId",
                table: "DebatePostLikes",
                columns: new[] { "UserId", "DebatePostId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebatePostLikes");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_FilmId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_SeriesId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");
        }
    }
}
