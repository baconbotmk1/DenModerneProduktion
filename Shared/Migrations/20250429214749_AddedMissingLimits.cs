using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SecurityGroupId",
                table: "TimeLimits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimits_SecurityGroupId",
                table: "TimeLimits",
                column: "SecurityGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLimits_SecurityGroups_SecurityGroupId",
                table: "TimeLimits",
                column: "SecurityGroupId",
                principalTable: "SecurityGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLimits_SecurityGroups_SecurityGroupId",
                table: "TimeLimits");

            migrationBuilder.DropIndex(
                name: "IX_TimeLimits_SecurityGroupId",
                table: "TimeLimits");

            migrationBuilder.DropColumn(
                name: "SecurityGroupId",
                table: "TimeLimits");
        }
    }
}
