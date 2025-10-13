using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport.Migrations
{
    /// <inheritdoc />
    public partial class MigV17_access_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_IdentityUserId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_IdentityUserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_AspNetUsers_IdentityUserId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_IdentityUserId",
                table: "Cars",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_IdentityUserId",
                table: "Drivers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_AspNetUsers_IdentityUserId",
                table: "Parents",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_IdentityUserId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_IdentityUserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_AspNetUsers_IdentityUserId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_IdentityUserId",
                table: "Cars",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_IdentityUserId",
                table: "Drivers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_AspNetUsers_IdentityUserId",
                table: "Parents",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
