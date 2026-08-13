using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dimakotso_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificateModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CertificateCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CertificateTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentEnrollmentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    AssessorId = table.Column<int>(type: "int", nullable: true),
                    SelectedAssessorQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineCapacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeedsFitnessCertificate = table.Column<bool>(type: "bit", nullable: false),
                    NoExpiryAdviseRenew = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TheoreticalPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PracticalPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    FinalPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    AssessmentMetadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationAndCompliance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Assessors_AssessorId",
                        column: x => x.AssessorId,
                        principalTable: "Assessors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Certificates_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificates_StudentEnrollments_StudentEnrollmentId",
                        column: x => x.StudentEnrollmentId,
                        principalTable: "StudentEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MachineAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MachineRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineRestrictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateAttachments",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false),
                    MachineAttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateAttachments", x => new { x.CertificateId, x.MachineAttachmentId });
                    table.ForeignKey(
                        name: "FK_CertificateAttachments_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateAttachments_MachineAttachments_MachineAttachmentId",
                        column: x => x.MachineAttachmentId,
                        principalTable: "MachineAttachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CertificateRestrictions",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false),
                    MachineRestrictionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateRestrictions", x => new { x.CertificateId, x.MachineRestrictionId });
                    table.ForeignKey(
                        name: "FK_CertificateRestrictions_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateRestrictions_MachineRestrictions_MachineRestrictionId",
                        column: x => x.MachineRestrictionId,
                        principalTable: "MachineRestrictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateAttachments_MachineAttachmentId",
                table: "CertificateAttachments",
                column: "MachineAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRestrictions_MachineRestrictionId",
                table: "CertificateRestrictions",
                column: "MachineRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_AssessorId",
                table: "Certificates",
                column: "AssessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CourseId",
                table: "Certificates",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_StudentEnrollmentId",
                table: "Certificates",
                column: "StudentEnrollmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateAttachments");

            migrationBuilder.DropTable(
                name: "CertificateRestrictions");

            migrationBuilder.DropTable(
                name: "MachineAttachments");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "MachineRestrictions");
        }
    }
}
