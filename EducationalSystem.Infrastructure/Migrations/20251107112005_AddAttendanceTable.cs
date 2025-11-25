using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbAttendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MarkedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MarkedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAttendance_AspNetUsers_MarkedBy",
                        column: x => x.MarkedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbAttendance_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbAttendance_TbGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TbGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAttendance_GroupId",
                table: "TbAttendance",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttendance_MarkedBy",
                table: "TbAttendance",
                column: "MarkedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttendance_StudentId",
                table: "TbAttendance",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbAttendance");
        }
    }
}
