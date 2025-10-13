using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport.Migrations
{
    /// <inheritdoc />
    public partial class MigV19_Del_Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Drivers_DriverId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Parents_ParentId",
                table: "Invites");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Drivers_DriverId",
                table: "Invites",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Parents_ParentId",
                table: "Invites",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Drivers_DriverId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Parents_ParentId",
                table: "Invites");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Drivers_DriverId",
                table: "Invites",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Parents_ParentId",
                table: "Invites",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "ParentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
