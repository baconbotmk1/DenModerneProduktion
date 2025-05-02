using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class DataLimitValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceDataLimit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MinimumLimit = table.Column<double>(type: "double", nullable: false),
                    MaximumLimit = table.Column<double>(type: "double", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDataLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceDataLimit_DeviceDataTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceDataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_DeviceDataLimit_TypeId",
                table: "DeviceDataLimit",
                column: "TypeId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LimitValueAssignment");

            migrationBuilder.DropTable(
                name: "DeviceDataLimit");
        }
    }
}
