using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cadastres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadastres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceSharedCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSharedCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FromDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ToDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CadastreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Cadastres_CadastreId",
                        column: x => x.CadastreId,
                        principalTable: "Cadastres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceDataTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDataTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceDataTypes_DeviceSharedCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DeviceSharedCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceEventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceEventTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceEventTypes_DeviceSharedCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DeviceSharedCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInfoTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInfoTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceInfoTypes_DeviceSharedCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DeviceSharedCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityGroups_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimeLimitWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLimitWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeLimitWeeks_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenceId = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceType = table.Column<string>(type: "TEXT", nullable: true),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceDatas_DeviceDataTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceDataTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceDatas_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", nullable: true),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceEvents_DeviceEventTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceEventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceEvents_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceInfos_DeviceInfoTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceInfoTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceInfos_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceRecordings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Started = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<float>(type: "REAL", nullable: true),
                    Filepath = table.Column<string>(type: "TEXT", nullable: false),
                    Finished = table.Column<bool>(type: "INTEGER", nullable: false),
                    Converted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRecordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceRecordings_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityGroupDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SecurityGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGroupDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityGroupDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupDevices_SecurityGroups_SecurityGroupId",
                        column: x => x.SecurityGroupId,
                        principalTable: "SecurityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupDevices_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SecurityGroupPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SecurityGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGroupPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityGroupPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupPermissions_SecurityGroups_SecurityGroupId",
                        column: x => x.SecurityGroupId,
                        principalTable: "SecurityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeLimitWeekDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    WeekId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLimitWeekDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeLimitWeekDays_TimeLimitWeeks_WeekId",
                        column: x => x.WeekId,
                        principalTable: "TimeLimitWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UniqueCode = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDevices_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDevices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSecurityGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    SecurityGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurityGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecurityGroups_SecurityGroups_SecurityGroupId",
                        column: x => x.SecurityGroupId,
                        principalTable: "SecurityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSecurityGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityGroupSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SecurityGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGroupSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityGroupSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupSections_SecurityGroups_SecurityGroupId",
                        column: x => x.SecurityGroupId,
                        principalTable: "SecurityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupSections_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSections_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeLimitWeekDayTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ToTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WeekDayId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLimitWeekDayTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeLimitWeekDayTimes_TimeLimitWeekDays_WeekDayId",
                        column: x => x.WeekDayId,
                        principalTable: "TimeLimitWeekDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityGroupRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SecurityGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGroupRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityGroupRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupRooms_SecurityGroups_SecurityGroupId",
                        column: x => x.SecurityGroupId,
                        principalTable: "SecurityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityGroupRooms_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRooms_TimeLimits_TimeLimitId",
                        column: x => x.TimeLimitId,
                        principalTable: "TimeLimits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRooms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeLimitAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeLimitId = table.Column<int>(type: "INTEGER", nullable: false),
                    type = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    SecurityGroupDeviceId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecurityGroupRoomId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecurityGroupSectionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLimitAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeLimitAssignment_SecurityGroupDevices_SecurityGroupDeviceId",
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
                        name: "FK_TimeLimitAssignment_SecurityGroupSections_SecurityGroupSectionId",
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessCards_UserId",
                table: "AccessCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CadastreId",
                table: "Buildings",
                column: "CadastreId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDatas_DeviceId",
                table: "DeviceDatas",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDatas_TypeId",
                table: "DeviceDatas",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDataTypes_CategoryId",
                table: "DeviceDataTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceEvents_DeviceId",
                table: "DeviceEvents",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceEvents_TypeId",
                table: "DeviceEvents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceEventTypes_CategoryId",
                table: "DeviceEventTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInfos_DeviceId",
                table: "DeviceInfos",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInfos_TypeId",
                table: "DeviceInfos",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInfoTypes_CategoryId",
                table: "DeviceInfoTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceRecordings_DeviceId",
                table: "DeviceRecordings",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_TypeId",
                table: "Devices",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SectionId",
                table: "Rooms",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_BuildingId",
                table: "Sections",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupDevices_DeviceId",
                table: "SecurityGroupDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupDevices_SecurityGroupId",
                table: "SecurityGroupDevices",
                column: "SecurityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupDevices_TimeLimitId",
                table: "SecurityGroupDevices",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupPermissions_PermissionId",
                table: "SecurityGroupPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupPermissions_SecurityGroupId",
                table: "SecurityGroupPermissions",
                column: "SecurityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupRooms_RoomId",
                table: "SecurityGroupRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupRooms_SecurityGroupId",
                table: "SecurityGroupRooms",
                column: "SecurityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupRooms_TimeLimitId",
                table: "SecurityGroupRooms",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroups_TimeLimitId",
                table: "SecurityGroups",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupSections_SectionId",
                table: "SecurityGroupSections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupSections_SecurityGroupId",
                table: "SecurityGroupSections",
                column: "SecurityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGroupSections_TimeLimitId",
                table: "SecurityGroupSections",
                column: "TimeLimitId");

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
                name: "IX_TimeLimitWeekDays_WeekId",
                table: "TimeLimitWeekDays",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitWeekDayTimes_WeekDayId",
                table: "TimeLimitWeekDayTimes",
                column: "WeekDayId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLimitWeeks_TimeLimitId",
                table: "TimeLimitWeeks",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_DeviceId",
                table: "UserDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_TimeLimitId",
                table: "UserDevices",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_RoomId",
                table: "UserRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_TimeLimitId",
                table: "UserRooms",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRooms_UserId",
                table: "UserRooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TimeLimitId",
                table: "Users",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSections_SectionId",
                table: "UserSections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSections_TimeLimitId",
                table: "UserSections",
                column: "TimeLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSections_UserId",
                table: "UserSections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecurityGroups_SecurityGroupId",
                table: "UserSecurityGroups",
                column: "SecurityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecurityGroups_UserId",
                table: "UserSecurityGroups",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessCards");

            migrationBuilder.DropTable(
                name: "DeviceDatas");

            migrationBuilder.DropTable(
                name: "DeviceEvents");

            migrationBuilder.DropTable(
                name: "DeviceInfos");

            migrationBuilder.DropTable(
                name: "DeviceRecordings");

            migrationBuilder.DropTable(
                name: "SecurityGroupPermissions");

            migrationBuilder.DropTable(
                name: "TimeLimitAssignment");

            migrationBuilder.DropTable(
                name: "TimeLimitWeekDayTimes");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "UserRooms");

            migrationBuilder.DropTable(
                name: "UserSections");

            migrationBuilder.DropTable(
                name: "UserSecurityGroups");

            migrationBuilder.DropTable(
                name: "DeviceDataTypes");

            migrationBuilder.DropTable(
                name: "DeviceEventTypes");

            migrationBuilder.DropTable(
                name: "DeviceInfoTypes");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "SecurityGroupDevices");

            migrationBuilder.DropTable(
                name: "SecurityGroupRooms");

            migrationBuilder.DropTable(
                name: "SecurityGroupSections");

            migrationBuilder.DropTable(
                name: "TimeLimitWeekDays");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DeviceSharedCategories");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "SecurityGroups");

            migrationBuilder.DropTable(
                name: "TimeLimitWeeks");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "TimeLimits");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Cadastres");
        }
    }
}
