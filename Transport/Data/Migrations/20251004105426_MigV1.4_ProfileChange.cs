using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigV14_ProfileChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Parents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Drivers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_IdentityUserId",
                table: "Parents",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_IdentityUserId",
                table: "Drivers",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_IdentityUserId",
                table: "Cars",
                column: "IdentityUserId");

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

            migrationBuilder.DropIndex(
                name: "IX_Parents_IdentityUserId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_IdentityUserId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Cars_IdentityUserId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Cars");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
