using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScannerWebAppUpdate.Migrations
{
    /// <inheritdoc />
    public partial class SignedOffAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignedOff",
                table: "JobParts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedOff",
                table: "JobParts");
        }
    }
}
