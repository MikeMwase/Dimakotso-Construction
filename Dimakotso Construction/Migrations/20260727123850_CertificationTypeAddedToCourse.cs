using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dimakotso_Construction.Migrations
{
    /// <inheritdoc />
    public partial class CertificationTypeAddedToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Certification",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certification",
                table: "Courses");
        }
    }
}
