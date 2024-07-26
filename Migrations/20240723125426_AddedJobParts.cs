using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScannerWebAppUpdate.Migrations
{
    /// <inheritdoc />
    public partial class AddedJobParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobParts",
                columns: table => new
                {
                    JobPartId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartId = table.Column<int>(type: "INTEGER", nullable: false),
                    JobId = table.Column<int>(type: "INTEGER", nullable: false),
                    truckId = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    AssignedQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PartType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobParts", x => x.JobPartId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobParts");
        }
    }
}
