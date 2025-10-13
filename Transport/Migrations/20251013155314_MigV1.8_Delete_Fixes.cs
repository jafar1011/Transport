using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport.Migrations
{
    /// <inheritdoc />
    public partial class MigV18_Delete_Fixes : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Drivers_DriverId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Parents_ParentId",
                table: "Invites");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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
    }
}
