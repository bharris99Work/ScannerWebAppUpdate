using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScannerWebAppUpdate.Migrations
{
    /// <inheritdoc />
    public partial class JobHistoryNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_jobHistories",
                table: "jobHistories");

            migrationBuilder.RenameTable(
                name: "jobHistories",
                newName: "JobHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobHistory",
                table: "JobHistory",
                column: "JobHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobHistory",
                table: "JobHistory");

            migrationBuilder.RenameTable(
                name: "JobHistory",
                newName: "jobHistories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_jobHistories",
                table: "jobHistories",
                column: "JobHistoryId");
        }
    }
}
