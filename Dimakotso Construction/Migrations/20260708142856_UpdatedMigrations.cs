using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dimakotso_Construction.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentEnrollments_WorkplacePlacements_WorkplacePlacementId",
                table: "StudentEnrollments");

            migrationBuilder.RenameColumn(
                name: "WorkplacePlacementId",
                table: "StudentEnrollments",
                newName: "EmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentEnrollments_WorkplacePlacementId",
                table: "StudentEnrollments",
                newName: "IX_StudentEnrollments_EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentEnrollments_Employers_EmployerId",
                table: "StudentEnrollments",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentEnrollments_Employers_EmployerId",
                table: "StudentEnrollments");

            migrationBuilder.RenameColumn(
                name: "EmployerId",
                table: "StudentEnrollments",
                newName: "WorkplacePlacementId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentEnrollments_EmployerId",
                table: "StudentEnrollments",
                newName: "IX_StudentEnrollments_WorkplacePlacementId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentEnrollments_WorkplacePlacements_WorkplacePlacementId",
                table: "StudentEnrollments",
                column: "WorkplacePlacementId",
                principalTable: "WorkplacePlacements",
                principalColumn: "Id");
        }
    }
}
