using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport.Migrations
{
    /// <inheritdoc />
    public partial class MigV11_Added_Rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverPosts_AspNetUsers_IdentityUserId",
                table: "DriverPosts");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "DriverPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    RatingValue = table.Column<float>(type: "REAL", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Ratings_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverPosts_DriverId",
                table: "DriverPosts",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_DriverId",
                table: "Ratings",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverPosts_AspNetUsers_IdentityUserId",
                table: "DriverPosts",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverPosts_Drivers_DriverId",
                table: "DriverPosts",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverPosts_AspNetUsers_IdentityUserId",
                table: "DriverPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverPosts_Drivers_DriverId",
                table: "DriverPosts");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_DriverPosts_DriverId",
                table: "DriverPosts");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "DriverPosts");

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverPosts_AspNetUsers_IdentityUserId",
                table: "DriverPosts",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
