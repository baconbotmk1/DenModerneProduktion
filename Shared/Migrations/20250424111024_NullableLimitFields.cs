using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class NullableLimitFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Buildings_BuildingId",
                table: "DeviceDataLimit");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Cadastres_CadastreId",
                table: "DeviceDataLimit");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Rooms_RoomId",
                table: "DeviceDataLimit");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Sections_SectionId",
                table: "DeviceDataLimit");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CadastreId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Buildings_BuildingId",
                table: "DeviceDataLimit",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Cadastres_CadastreId",
                table: "DeviceDataLimit",
                column: "CadastreId",
                principalTable: "Cadastres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Rooms_RoomId",
                table: "DeviceDataLimit",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Sections_SectionId",
                table: "DeviceDataLimit",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Buildings_BuildingId",
                table: "DeviceDataLimit");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Cadastres_CadastreId",
                table: "DeviceDataLimit");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Rooms_RoomId",
                table: "DeviceDataLimit");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceDataLimit_Sections_SectionId",
                table: "DeviceDataLimit");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CadastreId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Buildings_BuildingId",
                table: "DeviceDataLimit",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Cadastres_CadastreId",
                table: "DeviceDataLimit",
                column: "CadastreId",
                principalTable: "Cadastres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Rooms_RoomId",
                table: "DeviceDataLimit",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceDataLimit_Sections_SectionId",
                table: "DeviceDataLimit",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
