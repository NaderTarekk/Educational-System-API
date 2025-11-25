using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsGroupUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbGroups_AspNetUsers_AssistantId",
                table: "TbGroups");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_TbGroups_AspNetUsers_AssistantId",
                table: "TbGroups",
                column: "AssistantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
