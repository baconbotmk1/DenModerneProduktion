using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeLimitAssignment");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecurityGroupDeviceId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecurityGroupRoomId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecurityGroupSectionId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserDeviceId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRoomId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserSectionId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_RoomId",
                table: "TimeLimits",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_SecurityGroupDeviceId",
                table: "TimeLimits",
                column: "SecurityGroupDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_SecurityGroupRoomId",
                table: "TimeLimits",
                column: "SecurityGroupRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_SecurityGroupSectionId",
                table: "TimeLimits",
                column: "SecurityGroupSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_UserDeviceId",
                table: "TimeLimits",
                column: "UserDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_UserRoomId",
                table: "TimeLimits",
                column: "UserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_UserSectionId",
                table: "TimeLimits",
                column: "UserSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_Rooms_RoomId",
                table: "TimeLimits",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_SecurityGroupDevices_SecurityGroupDeviceId",
                table: "TimeLimits",
                column: "SecurityGroupDeviceId",
                principalTable: "SecurityGroupDevices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_SecurityGroupRooms_SecurityGroupRoomId",
                table: "TimeLimits",
                column: "SecurityGroupRoomId",
                principalTable: "SecurityGroupRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_SecurityGroupSections_SecurityGroupSectionId",
                table: "TimeLimits",
                column: "SecurityGroupSectionId",
                principalTable: "SecurityGroupSections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_UserDevices_UserDeviceId",
                table: "TimeLimits",
                column: "UserDeviceId",
                principalTable: "UserDevices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_UserRooms_UserRoomId",
                table: "TimeLimits",
                column: "UserRoomId",
                principalTable: "UserRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_UserSections_UserSectionId",
                table: "TimeLimits",
                column: "UserSectionId",
                principalTable: "UserSections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_Rooms_RoomId",
                table: "TimeLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_SecurityGroupDevices_SecurityGroupDeviceId",
                table: "TimeLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_SecurityGroupRooms_SecurityGroupRoomId",
                table: "TimeLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_SecurityGroupSections_SecurityGroupSectionId",
                table: "TimeLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_UserDevices_UserDeviceId",
                table: "TimeLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_UserRooms_UserRoomId",
                table: "TimeLimits");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_UserSections_UserSectionId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_RoomId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_SecurityGroupDeviceId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_SecurityGroupRoomId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_SecurityGroupSectionId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_UserDeviceId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_UserRoomId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_UserSectionId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "SecurityGroupDeviceId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "SecurityGroupRoomId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "SecurityGroupSectionId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "UserDeviceId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "UserRoomId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "UserSectionId",
                table: "TimeLimits");

            migrationBuilder.CreateTable(
                name: "TimeLimitAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeLimitId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(21)", maxLength: 21, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityGroupDeviceId = table.Column<int>(type: "int", nullable: true),
                    SecurityGroupRoomId = table.Column<int>(type: "int", nullable: true),
                    SecurityGroupSectionId = table.Column<int>(type: "int", nullable: true),
                    UserDeviceId = table.Column<int>(type: "int", nullable: true),
                    UserRoomId = table.Column<int>(type: "int", nullable: true),
                    UserSectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLimitAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_SecurityGroupDevices_SecurityGroupDevice~",
                        column: x => x.SecurityGroupDeviceId,
                        principalTable: "SecurityGroupDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_SecurityGroupRooms_SecurityGroupRoomId",
                        column: x => x.SecurityGroupRoomId,
                        principalTable: "SecurityGroupRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_SecurityGroupSections_SecurityGroupSecti~",
                        column: x => x.SecurityGroupSectionId,
                        principalTable: "SecurityGroupSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_UserDevices_UserDeviceId",
                        column: x => x.UserDeviceId,
                        principalTable: "UserDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_UserRooms_UserRoomId",
                        column: x => x.UserRoomId,
                        principalTable: "UserRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_UserSections_UserSectionId",
                        column: x => x.UserSectionId,
                        principalTable: "UserSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_SecurityGroupDeviceId",
                table: "TimeLimitAssignment",
                column: "SecurityGroupDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_SecurityGroupRoomId",
                table: "TimeLimitAssignment",
                column: "SecurityGroupRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_SecurityGroupSectionId",
                table: "TimeLimitAssignment",
                column: "SecurityGroupSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_TimeLimitId",
                table: "TimeLimitAssignment",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_UserDeviceId",
                table: "TimeLimitAssignment",
                column: "UserDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_UserRoomId",
                table: "TimeLimitAssignment",
                column: "UserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitAssignment_UserSectionId",
                table: "TimeLimitAssignment",
                column: "UserSectionId");
        }
    }
}
