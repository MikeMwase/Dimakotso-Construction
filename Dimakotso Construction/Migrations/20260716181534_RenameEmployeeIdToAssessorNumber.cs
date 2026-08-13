using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dimakotso_Construction.Migrations
{
    /// <inheritdoc />
    public partial class RenameEmployeeIdToAssessorNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Assessors",
                newName: "AssessorNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssessorNumber",
                table: "Assessors",
                newName: "EmployeeId");
        }
    }
}
