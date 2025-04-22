using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class SomeMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Devices",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeviceMqttMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataTypeId = table.Column<int>(type: "int", nullable: true),
                    InfoTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMqttMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceMqttMaps_DeviceDataTypes_DataTypeId",
                        column: x => x.DataTypeId,
                        principalTable: "DeviceDataTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceMqttMaps_DeviceInfoTypes_InfoTypeId",
                        column: x => x.InfoTypeId,
                        principalTable: "DeviceInfoTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceMqttMaps_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMqttMaps_DataTypeId",
                table: "DeviceMqttMaps",
                column: "DataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMqttMaps_DeviceTypeId",
                table: "DeviceMqttMaps",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMqttMaps_InfoTypeId",
                table: "DeviceMqttMaps",
                column: "InfoTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMqttMaps");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Devices");
        }
    }
}
