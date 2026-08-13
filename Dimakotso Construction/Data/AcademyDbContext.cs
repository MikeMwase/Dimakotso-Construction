using Dimakotso_Construction.Models;
using Microsoft.AspNetCore.Identity; // Added for IdentityUser
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Added for IdentityDbContext
using Microsoft.EntityFrameworkCore;

namespace Dimakotso_Construction.Data
{
    // Inherit from IdentityDbContext<IdentityUser> instead of DbContext
    public class AcademyDbContext : IdentityDbContext<IdentityUser>
    {
        public AcademyDbContext(DbContextOptions<AcademyDbContext> options) : base(options) { }
        public DbSet<Employer> Employers { get; set; } 
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<StudentEnrollment> StudentEnrollments { get; set; }
        public DbSet<Assessors> Assessors { get; set; }
        public DbSet<AssessorQualification> AssessorQualifications { get; set; }
        public DbSet<AssessorCourse> AssessorCourses { get; set; }
        public DbSet<WorkplacePlacement> WorkplacePlacements { get; set; }
        public DbSet<ComplianceDocument> ComplianceDocuments { get; set; }

        // =========================================================================
        // ADDED: Certificate Module DbSets
        // =========================================================================
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<MachineAttachment> MachineAttachments { get; set; }
        public DbSet<CertificateAttachment> CertificateAttachments { get; set; }
        public DbSet<MachineRestriction> MachineRestrictions { get; set; }
        public DbSet<CertificateRestriction> CertificateRestrictions { get; set; }
        // =========================================================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Must call base.OnModelCreating to configure Identity tables
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new{sc.StudentEnrollmentId,sc.CourseId});

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.StudentEnrollment)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentEnrollmentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.StudentEnrollment)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentEnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure One-to-One structural mapping between Enrollment and Compliance Documents
            modelBuilder.Entity<StudentEnrollment>()
                .HasOne(s => s.VerificationDocuments)
                .WithOne(d => d.StudentEnrollment)
                .HasForeignKey<ComplianceDocument>(d => d.StudentEnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enforce registration reference indices constraints
            modelBuilder.Entity<StudentEnrollment>()
                .HasIndex(s => s.RegistrationNumber)
                .IsUnique();

            modelBuilder.Entity<StudentEnrollment>()
                .HasIndex(s => s.IdentificationNumber)
                .IsUnique();

            // Assessor-to-Course accreditation mapping
            modelBuilder.Entity<AssessorCourse>()
                .HasKey(ac => new { ac.AssessorId, ac.CourseId });

            modelBuilder.Entity<AssessorCourse>()
                .HasOne(ac => ac.Assessor)
                .WithMany(a => a.AssessorCourses)
                .HasForeignKey(ac => ac.AssessorId);

            modelBuilder.Entity<AssessorCourse>()
                .HasOne(ac => ac.Course)
                .WithMany(c => c.AssessorCourses)
                .HasForeignKey(ac => ac.CourseId);

            // =========================================================================
            // ADDED: Certificate Mappings, Relationships & Decimal Precision
            // =========================================================================

            // 1. CertificateAttachment Join Table Configuration (Many-to-Many)
            modelBuilder.Entity<CertificateAttachment>()
                .HasKey(ca => new { ca.CertificateId, ca.MachineAttachmentId });

            modelBuilder.Entity<CertificateAttachment>()
                .HasOne(ca => ca.Certificate)
                .WithMany(c => c.CertificateAttachments)
                .HasForeignKey(ca => ca.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateAttachment>()
                .HasOne(ca => ca.MachineAttachment)
                .WithMany()
                .HasForeignKey(ca => ca.MachineAttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. CertificateRestriction Join Table Configuration (Many-to-Many)
            modelBuilder.Entity<CertificateRestriction>()
                .HasKey(cr => new { cr.CertificateId, cr.MachineRestrictionId });

            modelBuilder.Entity<CertificateRestriction>()
                .HasOne(cr => cr.Certificate)
                .WithMany(c => c.CertificateRestrictions)
                .HasForeignKey(cr => cr.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateRestriction>()
                .HasOne(cr => cr.MachineRestriction)
                .WithMany()
                .HasForeignKey(cr => cr.MachineRestrictionId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Primary Certificate Foreign Keys & Delete Behaviors
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.StudentEnrollment)
                .WithMany()
                .HasForeignKey(c => c.StudentEnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Course)
                .WithMany()
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Assessor)
                .WithMany()
                .HasForeignKey(c => c.AssessorId)
                .OnDelete(DeleteBehavior.SetNull);

            // 4. Decimal Precision Configuration for Scores
            modelBuilder.Entity<Certificate>()
                .Property(c => c.TheoreticalPercentage)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Certificate>()
                .Property(c => c.PracticalPercentage)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Certificate>()
                .Property(c => c.FinalPercentage)
                .HasColumnType("decimal(5,2)");
            // =========================================================================
        }
    }
}