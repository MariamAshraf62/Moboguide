using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moboguide.Migrations
{
    /// <inheritdoc />
    public partial class Last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mobiles",
                columns: table => new
                {
                    MobileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Producer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetworkTech = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    materials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SIM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayProtection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayResolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChipSet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDCard = table.Column<bool>(type: "bit", nullable: false),
                    Storage_Memory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainCamera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainCameraVideo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainCameraFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrontCamera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrontCameraVideo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speakers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jack = table.Column<bool>(type: "bit", nullable: false),
                    WLAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sensors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryCharging = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvgRate = table.Column<float>(type: "real", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mobiles", x => x.MobileName);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comments_mobiles_MobileName",
                        column: x => x.MobileName,
                        principalTable: "mobiles",
                        principalColumn: "MobileName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mobilesUsers",
                columns: table => new
                {
                    MobileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mobilesUsers", x => new { x.MobileName, x.UserId });
                    table.ForeignKey(
                        name: "FK_mobilesUsers_mobiles_MobileName",
                        column: x => x.MobileName,
                        principalTable: "mobiles",
                        principalColumn: "MobileName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mobilesUsers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_MobileName",
                table: "comments",
                column: "MobileName");

            migrationBuilder.CreateIndex(
                name: "IX_comments_UserId",
                table: "comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_mobilesUsers_UserId",
                table: "mobilesUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "mobilesUsers");

            migrationBuilder.DropTable(
                name: "mobiles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
