using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToStudentGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "TbGroups",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "TbGroups");
        }
    }
}
