using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScannerWebAppUpdate.Migrations
{
    /// <inheritdoc />
    public partial class POTabelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "POParts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "POParts");
        }
    }
}
