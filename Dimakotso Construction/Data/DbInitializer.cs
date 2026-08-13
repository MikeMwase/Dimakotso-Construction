using Dimakotso_Construction.Models;
using Dimakotso_Construction.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Dimakotso_Construction.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            AcademyDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // ❌ REMOVED: context.Database.EnsureCreated()
            // Program.cs already calls context.Database.Migrate()
            // Running both causes silent seeding failures

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
            if (!context.Employers.Any())
            {
                var employers = new Employer[]
                {
                    new Employer { CompanyName = "2210 Aircon N Projects",  Sector = "Air Conditioning", Size = "N/A", City = "N/A", Province = "N/A", Status = "Active" },
                    new Employer { CompanyName = "3g Concrete",             Sector = "Construction",     Size = "N/A", City = "N/A", Province = "N/A", Status = "Active" },
                    new Employer { CompanyName = "3q",                      Sector = "General",          Size = "N/A", City = "N/A", Province = "N/A", Status = "Active" },
                    new Employer { CompanyName = "A P E Pumps",             Sector = "Engineering",      Size = "N/A", City = "N/A", Province = "N/A", Status = "Active" },
                    new Employer { CompanyName = "ADHOC WORKS INTEGRATED",  Sector = "General",          Size = "N/A", City = "N/A", Province = "N/A", Status = "Active" }
                };
                foreach (var e in employers) context.Employers.Add(e);
                context.SaveChanges();
                Console.WriteLine("✅ Employers seeded.");
            }

            // ── Assessors ──────────────────────────────────────────────────────
            if (!context.Assessors.Any())
            {
                var assessors = new Assessors[]
                {
                    new Assessors { FirstName = "Allan",   LastName = "Rowan",   Contact = "admin@spectramining.co.za", AssessorNumber = "16/ASS/005999/310112", Department = "N/A",          Status = "Inactive",      qualifications = "1 Active" },
                    new Assessors { FirstName = "Andries", LastName = "Botha",   Contact = "andries@nelko.co.za",       AssessorNumber = "EMPASSR19-113_50",      Department = "N/A",          Status = "Active",        qualifications = "3 Active" },
                    new Assessors { FirstName = "John",    LastName = "Sithole", Contact = "j.sithole@dimakotso.co.za", AssessorNumber = "EMPASSR22-001_10",      Department = "Construction", Status = "Active",        qualifications = "2 Active" },
                    new Assessors { FirstName = "Priya",   LastName = "Naidoo",  Contact = "p.naidoo@dimakotso.co.za",  AssessorNumber = "EMPASSR21-045_22",      Department = "H&S",          Status = "Active",        qualifications = "1 Active" },
                    new Assessors { FirstName = "Thabo",   LastName = "Molefe",  Contact = "t.molefe@dimakotso.co.za",  AssessorNumber = "EMPASSR20-078_33",      Department = "Electrical",   Status = "Expiring Soon", qualifications = "2 Active" }
                };
                foreach (var a in assessors) context.Assessors.Add(a);
                context.SaveChanges();
                Console.WriteLine("✅ Assessors seeded.");
            }

            // ── Workplace Placements ───────────────────────────────────────────
            if (!context.WorkplacePlacements.Any())
            {
                var placements = new WorkplacePlacement[]
{
    new WorkplacePlacement
    {
        HostEmployerName = "M Civils",
        CompanyVatNumber = "4001234567",
        PlacementPhysicalAddress = "15 Industria Road, Krugersdorp",
        Province = "Gauteng",
        MentorName = "Peter Mokoena",
        MentorJobTitle = "Construction Site Supervisor",
        MentorEmail = "pmokoena@mcivils.co.za",
        MentorCell = "0824567811"
    },

    new WorkplacePlacement
    {
        HostEmployerName = "Hanani (PPE Supplier)",
        CompanyVatNumber = "4123456789",
        PlacementPhysicalAddress = "18 Mining Supply Park, Rustenburg",
        Province = "North West",
        MentorName = "Sarah Jacobs",
        MentorJobTitle = "Supply Chain Coordinator",
        MentorEmail = "sarah.jacobs@hanani.co.za",
        MentorCell = "0836549871"
    },

    new WorkplacePlacement
    {
        HostEmployerName = "Diorama Trade and Invest",
        CompanyVatNumber = "4039876543",
        PlacementPhysicalAddress = "22 Training Avenue, Roodepoort",
        Province = "Gauteng",
        MentorName = "David Nkosi",
        MentorJobTitle = "Health & Safety Facilitator",
        MentorEmail = "d.nkosi@dioramatrade.co.za",
        MentorCell = "0843214567"
    },

    new WorkplacePlacement
    {
        HostEmployerName = "Volt Amp Technologies (Pty) Ltd",
        CompanyVatNumber = "4010293847",
        PlacementPhysicalAddress = "14 Industrial Complex Rd, Factoria",
        Province = "Gauteng",
        MentorName = "Thabo Letsobe",
        MentorJobTitle = "Master Artisan / Training Lead",
        MentorEmail = "thabo.l@voltamp.co.za",
        MentorCell = "0721234567"
    }
};

                context.WorkplacePlacements.AddRange(placements);
                context.SaveChanges();

                Console.WriteLine($"✅ {context.WorkplacePlacements.Count()} workplace placements seeded.");
            }

            // ── Courses ──────────────────────────────────────────────────────────────
            if (!context.Courses.Any())
            {
                var courses = new Course[]
                {
        new Course
        {
            CourseCode = "NTMP0050",
            CourseName = "Basic Aluminium TIG Welding",
            Description = "Fundamental TIG welding techniques for aluminium fabrication.",
            DurationHours = 40,
            Status = CourseStatus.Active,
            Type = CourseType.ShortCourse,
            Prerequisites = "Basic Welding Knowledge",
            LearningObjectives = "Perform safe aluminium TIG welding using industry best practices.",
            NQFLevel = 3,
            Credits = 8,
            UnitStandardNumber = "243063"
        },

        new Course
        {
            CourseCode = "NTMP422",
            CourseName = "Implement a Quality Management System on a Construction Project",
            Description = "Apply quality management principles on construction sites.",
            DurationHours = 24,
            Status = CourseStatus.Active,
            Type = CourseType.SkillsProgramme,
            Prerequisites = "Construction Experience",
            LearningObjectives = "Implement and monitor quality assurance procedures.",
            NQFLevel = 5,
            Credits = 12,
            UnitStandardNumber = "120379"
        },

        new Course
        {
            CourseCode = "NTMP001",
            CourseName = "Participate in the Implementation and Evaluation of a Safety and Health Management Programme",
            Description = "Occupational Health and Safety management.",
            DurationHours = 32,
            Status = CourseStatus.Active,
            Type = CourseType.SkillsProgramme,
            Prerequisites = "None",
            LearningObjectives = "Apply OHSA legislation and safety systems.",
            NQFLevel = 4,
            Credits = 10,
            UnitStandardNumber = "259639"
        },

        new Course
        {
            CourseCode = "ELEC001",
            CourseName = "Occupational Certificate: Electrician",
            Description = "Full occupational qualification for Electricians.",
            DurationHours = 2400,
            Status = CourseStatus.Active,
            Type = CourseType.Qualification,
            Prerequisites = "Grade 12 Mathematics",
            LearningObjectives = "Install, maintain and repair electrical systems.",
            NQFLevel = 4,
            Credits = 540,
            UnitStandardNumber = "91761"
        },

        new Course
        {
            CourseCode = "HSE001",
            CourseName = "Health and Safety Representative",
            Description = "Duties and responsibilities of Health and Safety Representatives.",
            DurationHours = 16,
            Status = CourseStatus.Active,
            Type = CourseType.ShortCourse,
            Prerequisites = "None",
            LearningObjectives = "Conduct inspections and identify workplace hazards.",
            NQFLevel = 2,
            Credits = 4,
            UnitStandardNumber = "259622"
        },

        new Course
        {
            CourseCode = "FAW001",
            CourseName = "First Aid Level 1",
            Description = "Emergency First Aid in the workplace.",
            DurationHours = 16,
            Status = CourseStatus.Active,
            Type = CourseType.ShortCourse,
            Prerequisites = "None",
            LearningObjectives = "Respond effectively to medical emergencies.",
            NQFLevel = 2,
            Credits = 3,
            UnitStandardNumber = "119567"
        },

        new Course
        {
            CourseCode = "WAH001",
            CourseName = "Working at Heights",
            Description = "Safe working procedures for elevated work.",
            DurationHours = 16,
            Status = CourseStatus.Active,
            Type = CourseType.ShortCourse,
            Prerequisites = "Medical Fitness Certificate",
            LearningObjectives = "Use fall arrest equipment safely.",
            NQFLevel = 3,
            Credits = 4,
            UnitStandardNumber = "229998"
        },

        new Course
        {
            CourseCode = "FIRE001",
            CourseName = "Fire Fighting",
            Description = "Basic fire prevention and firefighting techniques.",
            DurationHours = 8,
            Status = CourseStatus.Active,
            Type = CourseType.ShortCourse,
            Prerequisites = "None",
            LearningObjectives = "Use fire extinguishers correctly.",
            NQFLevel = 2,
            Credits = 2,
            UnitStandardNumber = "12484"
        },

        new Course
        {
            CourseCode = "SHEQ001",
            CourseName = "Introduction to SHEQ",
            Description = "Safety, Health, Environment and Quality fundamentals.",
            DurationHours = 24,
            Status = CourseStatus.Active,
            Type = CourseType.ShortCourse,
            Prerequisites = "None",
            LearningObjectives = "Understand integrated SHEQ systems.",
            NQFLevel = 4,
            Credits = 6,
            UnitStandardNumber = "9964"
        },

        new Course
        {
            CourseCode = "RIG001",
            CourseName = "Rigging and Slinging",
            Description = "Safe lifting and rigging operations.",
            DurationHours = 40,
            Status = CourseStatus.Active,
            Type = CourseType.SkillsProgramme,
            Prerequisites = "Medical Fitness",
            LearningObjectives = "Perform lifting operations safely.",
            NQFLevel = 3,
            Credits = 8,
            UnitStandardNumber = "260275"
        }
                };

                context.Courses.AddRange(courses);
                context.SaveChanges();

                Console.WriteLine($"✅ {courses.Length} courses seeded.");
            }

            // ── Student Enrollments ────────────────────────────────────────────
            if (!context.StudentEnrollments.Any())
            {
                var activePlacements = context.WorkplacePlacements.ToList();
                Console.WriteLine($"ℹ️  Placements available: {activePlacements.Count}");

                if (activePlacements.Count < 2)
                {
                    Console.WriteLine("❌ Not enough placements to seed students.");
                    return;
                }

                var students = new StudentEnrollment[]
                {
                    new StudentEnrollment
                    {
                        RegistrationNumber             = "DC-EL2026-001",
                        FirstNames                     = "Sibusiso Temba",
                        Surname                        = "Khumalo",
                        IdentificationNumber           = "9804155129087",
                        DateOfBirth                    = new DateTime(1998, 04, 15),
                        Gender                         = "Male",
                        Equity                         = EquityCode.BA,
                        Citizenship                    = CitizenStatusCode.SACitizen,
                        DisabilityStatus               = DisabilityCode.None,
                        CurrentEmployment              = EmploymentStatus.Unemployed,
                        HighestQualification           = "National Senior Certificate (Matric - Technical Track)",
                        Email                          = "sibu.khumalo@gmail.com",
                        MobileNumber                   = "0612345678",
                        HomeAddress                    = "Section 4, Kagiso, Krugersdorp",
                        PostalCode                     = "1754",
                        Status                         = EnrollmentStatus.ActiveTraining,
                        DateCreated                    = DateTime.UtcNow.AddMonths(-3),
                        HasConsentedToPopiaDataSharing = true,
                        EmployerId                     = activePlacements[0].Id
                    },
                    new StudentEnrollment
                    {
                        RegistrationNumber             = "DC-EL2026-002",
                        FirstNames                     = "Chantel Jade",
                        Surname                        = "Williams",
                        IdentificationNumber           = "0111230048081",
                        DateOfBirth                    = new DateTime(2001, 11, 23),
                        Gender                         = "Female",
                        Equity                         = EquityCode.BC,
                        Citizenship                    = CitizenStatusCode.SACitizen,
                        DisabilityStatus               = DisabilityCode.None,
                        CurrentEmployment              = EmploymentStatus.Employed,
                        HighestQualification           = "N2 Engineering Studies Certificate",
                        Email                          = "chantel.williams@outlook.com",
                        MobileNumber                   = "0739876541",
                        HomeAddress                    = "12 Witpoortjie Villas, Mindalore",
                        PostalCode                     = "1739",
                        Status                         = EnrollmentStatus.Registered,
                        DateCreated                    = DateTime.UtcNow.AddDays(-14),
                        HasConsentedToPopiaDataSharing = true,
                        EmployerId                     = activePlacements[1].Id
                    }
                };
                foreach (var s in students) context.StudentEnrollments.Add(s);
                context.SaveChanges();
                Console.WriteLine("✅ Students seeded.");
            }

            // ── Compliance Documents ───────────────────────────────────────────
            if (!context.ComplianceDocuments.Any())
            {
                var seededStudents = context.StudentEnrollments.ToList();
                Console.WriteLine($"ℹ️  Students available: {seededStudents.Count}");

                if (seededStudents.Count < 2)
                {
                    Console.WriteLine("❌ Not enough students to seed compliance docs.");
                    return;
                }

                var docs = new ComplianceDocument[]
                {
                    new ComplianceDocument { StudentEnrollmentId = seededStudents[0].Id, CertifiedIdPath = "/secure_storage/compliance/id_9804155129087_certified.pdf", HighestQualificationCertificatePath = "/secure_storage/compliance/qual_9804155129087_matric.pdf", SignedWorkplaceAgreementPath = "/secure_storage/compliance/agreement_9804155129087_voltamp.pdf"     },
                    new ComplianceDocument { StudentEnrollmentId = seededStudents[1].Id, CertifiedIdPath = "/secure_storage/compliance/id_0111230048081_certified.pdf", HighestQualificationCertificatePath = "/secure_storage/compliance/qual_0111230048081_n2.pdf",     SignedWorkplaceAgreementPath = "/secure_storage/compliance/agreement_0111230048081_gautenggrid.pdf" }
                };
                foreach (var d in docs) context.ComplianceDocuments.Add(d);
                context.SaveChanges();
                Console.WriteLine("✅ Compliance documents seeded.");
            }
        }
    }
}