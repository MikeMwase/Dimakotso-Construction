using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dimakotso_Construction.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkplacePlacements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostEmployerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyVatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlacementPhysicalAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorJobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorCell = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkplacePlacements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstNames = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equity = table.Column<int>(type: "int", nullable: false),
                    Citizenship = table.Column<int>(type: "int", nullable: false),
                    DisabilityStatus = table.Column<int>(type: "int", nullable: false),
                    CurrentEmployment = table.Column<int>(type: "int", nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TargetProgramTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaqaId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasConsentedToPopiaDataSharing = table.Column<bool>(type: "bit", nullable: false),
                    WorkplacePlacementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentEnrollments_WorkplacePlacements_WorkplacePlacementId",
                        column: x => x.WorkplacePlacementId,
                        principalTable: "WorkplacePlacements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComplianceDocuments",
                columns: table => new
                {
                    StudentEnrollmentId = table.Column<int>(type: "int", nullable: false),
                    CertifiedIdPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HighestQualificationCertificatePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignedWorkplaceAgreementPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceDocuments", x => x.StudentEnrollmentId);
                    table.ForeignKey(
                        name: "FK_ComplianceDocuments_StudentEnrollments_StudentEnrollmentId",
                        column: x => x.StudentEnrollmentId,
                        principalTable: "StudentEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollments_IdentificationNumber",
                table: "StudentEnrollments",
                column: "IdentificationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollments_RegistrationNumber",
                table: "StudentEnrollments",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollments_WorkplacePlacementId",
                table: "StudentEnrollments",
                column: "WorkplacePlacementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplianceDocuments");

            migrationBuilder.DropTable(
                name: "StudentEnrollments");

            migrationBuilder.DropTable(
                name: "WorkplacePlacements");
        }
    }
}
