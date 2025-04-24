using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAssignableDataLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LimitValueAssignment");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CadastreId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "DeviceDataLimit",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateTable(
                name: "LimitValueAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LimitValueId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(21)", maxLength: 21, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BuildingId = table.Column<int>(type: "int", nullable: true),
                    CadastreId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitValueAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LimitValueAssignment_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimitValueAssignment_Cadastres_CadastreId",
                        column: x => x.CadastreId,
                        principalTable: "Cadastres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimitValueAssignment_DeviceDataLimit_LimitValueId",
                        column: x => x.LimitValueId,
                        principalTable: "DeviceDataLimit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimitValueAssignment_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimitValueAssignment_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LimitValueAssignment_BuildingId",
                table: "LimitValueAssignment",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitValueAssignment_CadastreId",
                table: "LimitValueAssignment",
                column: "CadastreId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitValueAssignment_LimitValueId",
                table: "LimitValueAssignment",
                column: "LimitValueId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitValueAssignment_RoomId",
                table: "LimitValueAssignment",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitValueAssignment_SectionId",
                table: "LimitValueAssignment",
                column: "SectionId");
        }
    }
}
