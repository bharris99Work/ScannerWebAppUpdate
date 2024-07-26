using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScannerWebAppUpdate.Migrations
{
    /// <inheritdoc />
    public partial class RemovePartType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartType",
                table: "JobParts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartType",
                table: "JobParts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
