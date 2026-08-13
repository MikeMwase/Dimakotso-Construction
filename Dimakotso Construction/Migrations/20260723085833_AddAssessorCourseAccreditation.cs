using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dimakotso_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessorCourseAccreditation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentEnrollmentId1",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssessorCourses",
                columns: table => new
                {
                    AssessorId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    AccreditationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessorCourses", x => new { x.AssessorId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_AssessorCourses_Assessors_AssessorId",
                        column: x => x.AssessorId,
                        principalTable: "Assessors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessorCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_StudentEnrollmentId1",
                table: "Certificates",
                column: "StudentEnrollmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_AssessorCourses_CourseId",
                table: "AssessorCourses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_StudentEnrollments_StudentEnrollmentId1",
                table: "Certificates",
                column: "StudentEnrollmentId1",
                principalTable: "StudentEnrollments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_StudentEnrollments_StudentEnrollmentId1",
                table: "Certificates");

            migrationBuilder.DropTable(
                name: "AssessorCourses");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_StudentEnrollmentId1",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "StudentEnrollmentId1",
                table: "Certificates");
        }
    }
}
