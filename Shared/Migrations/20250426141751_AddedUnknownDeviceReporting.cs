using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnknownDeviceReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MinimumLimit",
                table: "DeviceDataLimit",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "MaximumLimit",
                table: "DeviceDataLimit",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CadastreId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnknownMqttDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstFoundAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastFoundAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnknownMqttDevices", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDataLimit_BuildingId",
                table: "DeviceDataLimit",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDataLimit_CadastreId",
                table: "DeviceDataLimit",
                column: "CadastreId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDataLimit_RoomId",
                table: "DeviceDataLimit",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDataLimit_SectionId",
                table: "DeviceDataLimit",
                column: "SectionId");

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

            migrationBuilder.DropTable(
                name: "UnknownMqttDevices");

            migrationBuilder.DropIndex(
                name: "IX_DeviceDataLimit_BuildingId",
                table: "DeviceDataLimit");

            migrationBuilder.DropIndex(
                name: "IX_DeviceDataLimit_CadastreId",
                table: "DeviceDataLimit");

            migrationBuilder.DropIndex(
                name: "IX_DeviceDataLimit_RoomId",
                table: "DeviceDataLimit");

            migrationBuilder.DropIndex(
                name: "IX_DeviceDataLimit_SectionId",
                table: "DeviceDataLimit");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "DeviceDataLimit");

            migrationBuilder.DropColumn(
                name: "CadastreId",
                table: "DeviceDataLimit");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "DeviceDataLimit");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "DeviceDataLimit");

            migrationBuilder.AlterColumn<double>(
                name: "MinimumLimit",
                table: "DeviceDataLimit",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "MaximumLimit",
                table: "DeviceDataLimit",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }
    }
}
