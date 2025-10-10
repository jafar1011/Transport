using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport.Migrations
{
    /// <inheritdoc />
    public partial class MigV12_Fixed_Rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverPosts_AspNetUsers_IdentityUserId",
                table: "DriverPosts");

            migrationBuilder.DropIndex(
                name: "IX_DriverPosts_IdentityUserId",
                table: "DriverPosts");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "DriverPosts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Ratings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "DriverPosts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DriverPosts_IdentityUserId",
                table: "DriverPosts",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverPosts_AspNetUsers_IdentityUserId",
                table: "DriverPosts",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
