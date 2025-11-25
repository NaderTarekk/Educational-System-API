using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGroupsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TbGroups_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbGroups_AspNetUsers_AssistantId",
                table: "TbGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "TbUserGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbUserGroups_TbGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TbGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbUserGroups_GroupId",
                table: "TbUserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserGroups_UserId",
                table: "TbUserGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbGroups_AspNetUsers_AssistantId",
                table: "TbGroups",
                column: "AssistantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbGroups_AspNetUsers_AssistantId",
                table: "TbGroups");

            migrationBuilder.DropTable(
                name: "TbUserGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TbGroups_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "TbGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbGroups_AspNetUsers_AssistantId",
                table: "TbGroups",
                column: "AssistantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
