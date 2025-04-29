using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class MissingDeviceDataId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceDataId",
                table: "DeviceEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceEvents_DeviceDataId",
                table: "DeviceEvents",
                column: "DeviceDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceEvents_DeviceDatas_DeviceDataId",
                table: "DeviceEvents",
                column: "DeviceDataId",
                principalTable: "DeviceDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceEvents_DeviceDatas_DeviceDataId",
                table: "DeviceEvents");

            migrationBuilder.DropIndex(
                name: "IX_DeviceEvents_DeviceDataId",
                table: "DeviceEvents");

            migrationBuilder.DropColumn(
                name: "DeviceDataId",
                table: "DeviceEvents");
        }
    }
}
