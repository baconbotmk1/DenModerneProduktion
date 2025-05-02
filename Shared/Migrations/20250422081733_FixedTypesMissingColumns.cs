using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypesMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DeviceEventTypes",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "DeviceInfoTypes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Desc",
                table: "DeviceEventTypes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "DeviceDataTypes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "DeviceInfoTypes");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "DeviceDataTypes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DeviceEventTypes",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "DeviceEventTypes",
                keyColumn: "Desc",
                keyValue: null,
                column: "Desc",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Desc",
                table: "DeviceEventTypes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
