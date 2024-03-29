﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChargeLog.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Import",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NetworkCount = table.Column<int>(type: "int", nullable: false),
                    LocationCount = table.Column<int>(type: "int", nullable: false),
                    SessionCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Import", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Network",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    HaveAccount = table.Column<bool>(type: "bit", nullable: false),
                    IsPartner = table.Column<bool>(type: "bit", nullable: false),
                    DefaultChargeType = table.Column<int>(type: "int", nullable: false),
                    ImportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Network", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NetworkId = table.Column<int>(type: "int", nullable: false),
                    ImportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_Network_NetworkId",
                        column: x => x.NetworkId,
                        principalTable: "Network",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    KWh = table.Column<decimal>(type: "decimal(5,3)", precision: 5, scale: 3, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,3)", precision: 5, scale: 3, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(5,3)", precision: 5, scale: 3, nullable: false),
                    ChargeType = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ThroughNetworkId = table.Column<int>(type: "int", nullable: true),
                    ImportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Session_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Session_Network_ThroughNetworkId",
                        column: x => x.ThroughNetworkId,
                        principalTable: "Network",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupSession",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    SessionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSession", x => new { x.GroupsId, x.SessionsId });
                    table.ForeignKey(
                        name: "FK_GroupSession_Group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupSession_Session_SessionsId",
                        column: x => x.SessionsId,
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupSession_SessionsId",
                table: "GroupSession",
                column: "SessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_NetworkId",
                table: "Location",
                column: "NetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_CarId",
                table: "Session",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_LocationId",
                table: "Session",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_ThroughNetworkId",
                table: "Session",
                column: "ThroughNetworkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupSession");

            migrationBuilder.DropTable(
                name: "Import");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Network");
        }
    }
}
