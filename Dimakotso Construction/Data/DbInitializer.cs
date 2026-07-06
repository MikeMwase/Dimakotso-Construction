using Dimakotso_Construction.Models;
using Dimakotso_Construction.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dimakotso_Construction.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            AcademyDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // ── Roles ──────────────────────────────────────────────────────────
            if (!context.Roles.Any())
            {
                string[] roles = { "Admin", "Staff" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ── Users ──────────────────────────────────────────────────────────
            if (!context.Users.Any())
            {
                var admin = new IdentityUser
                {
                    UserName = "admin@dimakotso.co.za",
                    Email = "admin@dimakotso.co.za",
                    EmailConfirmed = true
                };
                var adminResult = await userManager.CreateAsync(admin, "Admin@1234");
                if (adminResult.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");

                var staff = new IdentityUser
                {
                    UserName = "staff@dimakotso.co.za",
                    Email = "staff@dimakotso.co.za",
                    EmailConfirmed = true
                };
                var staffResult = await userManager.CreateAsync(staff, "Staff@1234");
                if (staffResult.Succeeded)
                    await userManager.AddToRoleAsync(staff, "Staff");
            }

            // ── Employers ──────────────────────────────────────────────────────
            if (context.Employers.Any()) goto SeedAssessors;

            var employers = new Employer[]
            {
                new Employer { CompanyName = "2210 Aircon N Projects",        Sector = "Air Conditioning", Size = "N/A", City = "N/A", Province = "N/A", Status = "Active"  },
                new Employer { CompanyName = "3g Concrete",                   Sector = "Construction",     Size = "N/A", City = "N/A", Province = "N/A", Status = "Active"  },
                new Employer { CompanyName = "3q",                            Sector = "General",          Size = "N/A", City = "N/A", Province = "N/A", Status = "Active"  },
                new Employer { CompanyName = "A P E Pumps",                   Sector = "Engineering",      Size = "N/A", City = "N/A", Province = "N/A", Status = "Active"  },
                new Employer { CompanyName = "ADHOC WORKS INTEGRATED",        Sector = "General",          Size = "N/A", City = "N/A", Province = "N/A", Status = "Active"  }
            };
            foreach (var e in employers) context.Employers.Add(e);
            context.SaveChanges();

            // ── Assessors ──────────────────────────────────────────────────────
            SeedAssessors:
            if (context.Assessors.Any()) goto SeedPlacements;

            var assessors = new Assessors[]
            {
                new Assessors { FirstName = "Allan",   LastName = "Rowan",   Contact = "admin@spectramining.co.za",  EmployeeId = "16/ASS/005999/310112", Department = "N/A",          Status = "Inactive",      qualifications = "1 Active" },
                new Assessors { FirstName = "Andries", LastName = "Botha",   Contact = "andries@nelko.co.za",        EmployeeId = "EMPASSR19-113_50",      Department = "N/A",          Status = "Active",        qualifications = "3 Active" },
                new Assessors { FirstName = "John",    LastName = "Sithole", Contact = "j.sithole@dimakotso.co.za",  EmployeeId = "EMPASSR22-001_10",      Department = "Construction", Status = "Active",        qualifications = "2 Active" },
                new Assessors { FirstName = "Priya",   LastName = "Naidoo",  Contact = "p.naidoo@dimakotso.co.za",   EmployeeId = "EMPASSR21-045_22",      Department = "H&S",          Status = "Active",        qualifications = "1 Active" },
                new Assessors { FirstName = "Thabo",   LastName = "Molefe",  Contact = "t.molefe@dimakotso.co.za",   EmployeeId = "EMPASSR20-078_33",      Department = "Electrical",   Status = "Expiring Soon", qualifications = "2 Active" }
            };
            foreach (var a in assessors) context.Assessors.Add(a);
            context.SaveChanges();

            // ── Workplace Placements ───────────────────────────────────────────
            SeedPlacements:
            if (context.WorkplacePlacements.Any()) goto SeedStudents;

            var placements = new WorkplacePlacement[]
            {
                new WorkplacePlacement { HostEmployerName = "M Civils",                      PlacementPhysicalAddress = "Gauteng Region",                      MentorName = "Project Manager", MentorJobTitle = "Construction Site Supervisor"  },
                new WorkplacePlacement { HostEmployerName = "Hanani (PPE Supplier)",          PlacementPhysicalAddress = "Mining Industry Distribution Hub",    MentorName = "Sales Manager",   MentorJobTitle = "Supply Chain Coordinator"       },
                new WorkplacePlacement { HostEmployerName = "Diorama Trade and Invest",       PlacementPhysicalAddress = "Health & Safety Training Center",     MentorName = "Safety Officer",  MentorJobTitle = "H&S Facilitator"                },
                new WorkplacePlacement { HostEmployerName = "Volt Amp Technologies (Pty) Ltd",PlacementPhysicalAddress = "14 Industrial Complex Rd, Factoria",  MentorName = "Thabo Letsobe",   MentorJobTitle = "Master Artisan / Training Lead", CompanyVatNumber = "4010293847", Province = "Gauteng", MentorEmail = "thabo.l@voltamp.co.za", MentorCell = "0721234567" }
            };
            foreach (var p in placements) context.WorkplacePlacements.Add(p);
            context.SaveChanges();

            // ── Student Enrollments ────────────────────────────────────────────
            SeedStudents:
            if (context.StudentEnrollments.Any()) goto SeedCompliance;

            var activePlacements = context.WorkplacePlacements.ToList();
            var students = new StudentEnrollment[]
            {
                new StudentEnrollment
                {
                    RegistrationNumber      = "DC-EL2026-001",
                    FirstNames              = "Sibusiso Temba",
                    Surname                 = "Khumalo",
                    IdentificationNumber    = "9804155129087",
                    DateOfBirth             = new DateTime(1998, 04, 15),
                    Gender                  = "Male",
                    Equity                  = EquityCode.BA,
                    Citizenship             = CitizenStatusCode.SACitizen,
                    DisabilityStatus        = DisabilityCode.None,
                    CurrentEmployment       = EmploymentStatus.Unemployed,
                    HighestQualification    = "National Senior Certificate (Matric - Technical Track)",
                    Email                   = "sibu.khumalo@gmail.com",
                    MobileNumber            = "0612345678",
                    HomeAddress             = "Section 4, Kagiso, Krugersdorp",
                    PostalCode              = "1754",
                    TargetProgramTitle      = "Occupational Certificate: Electrician",
                    SaqaId                  = 91761,
                    Status                  = EnrollmentStatus.ActiveTraining,
                    DateCreated             = DateTime.UtcNow.AddMonths(-3),
                    HasConsentedToPopiaDataSharing = true,
                    WorkplacePlacementId    = activePlacements[0].Id
                },
                new StudentEnrollment
                {
                    RegistrationNumber      = "DC-EL2026-002",
                    FirstNames              = "Chantel Jade",
                    Surname                 = "Williams",
                    IdentificationNumber    = "0111230048081",
                    DateOfBirth             = new DateTime(2001, 11, 23),
                    Gender                  = "Female",
                    Equity                  = EquityCode.BC,
                    Citizenship             = CitizenStatusCode.SACitizen,
                    DisabilityStatus        = DisabilityCode.None,
                    CurrentEmployment       = EmploymentStatus.Employed,
                    HighestQualification    = "N2 Engineering Studies Certificate",
                    Email                   = "chantel.williams@outlook.com",
                    MobileNumber            = "0739876541",
                    HomeAddress             = "12 Witpoortjie Villas, Mindalore",
                    PostalCode              = "1739",
                    TargetProgramTitle      = "Occupational Certificate: Electrician",
                    SaqaId                  = 91761,
                    Status                  = EnrollmentStatus.Registered,
                    DateCreated             = DateTime.UtcNow.AddDays(-14),
                    HasConsentedToPopiaDataSharing = true,
                    WorkplacePlacementId    = activePlacements[1].Id
                }
            };
            foreach (var s in students) context.StudentEnrollments.Add(s);
            context.SaveChanges();

            // ── Compliance Documents ───────────────────────────────────────────
            SeedCompliance:
            if (context.ComplianceDocuments.Any()) return;

            var seededStudents = context.StudentEnrollments.ToList();
            var docs = new ComplianceDocument[]
            {
                new ComplianceDocument { StudentEnrollmentId = seededStudents[0].Id, CertifiedIdPath = "/secure_storage/compliance/id_9804155129087_certified.pdf",  HighestQualificationCertificatePath = "/secure_storage/compliance/qual_9804155129087_matric.pdf", SignedWorkplaceAgreementPath = "/secure_storage/compliance/agreement_9804155129087_voltamp.pdf"     },
                new ComplianceDocument { StudentEnrollmentId = seededStudents[1].Id, CertifiedIdPath = "/secure_storage/compliance/id_0111230048081_certified.pdf",  HighestQualificationCertificatePath = "/secure_storage/compliance/qual_0111230048081_n2.pdf",     SignedWorkplaceAgreementPath = "/secure_storage/compliance/agreement_0111230048081_gautenggrid.pdf" }
            };
            foreach (var d in docs) context.ComplianceDocuments.Add(d);
            context.SaveChanges();
        }
    }
}