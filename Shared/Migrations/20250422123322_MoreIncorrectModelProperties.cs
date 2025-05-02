using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class MoreIncorrectModelProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DeviceInfoTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DeviceEventTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DeviceDataTypes",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "DeviceInfoTypes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "DeviceInfos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "DeviceDatas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "DeviceInfoTypes");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "DeviceInfos");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "DeviceDatas");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DeviceInfoTypes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DeviceEventTypes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DeviceDataTypes",
                newName: "Type");
        }
    }
}
