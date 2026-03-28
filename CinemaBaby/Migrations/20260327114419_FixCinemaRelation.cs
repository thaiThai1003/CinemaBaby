using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaBaby.Migrations
{
    /// <inheritdoc />
    public partial class FixCinemaRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "Showtimes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_CinemaId",
                table: "Showtimes",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Cinemas_CinemaId",
                table: "Showtimes",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "CinemaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Cinemas_CinemaId",
                table: "Showtimes");

            migrationBuilder.DropIndex(
                name: "IX_Showtimes_CinemaId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "Showtimes");
        }
    }
}
