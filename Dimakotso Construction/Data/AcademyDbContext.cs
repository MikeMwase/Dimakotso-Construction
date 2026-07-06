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
        public DbSet<StudentEnrollment> StudentEnrollments { get; set; }
        public DbSet<Assessors> Assessors { get; set; }
        public DbSet<WorkplacePlacement> WorkplacePlacements { get; set; }
        public DbSet<ComplianceDocument> ComplianceDocuments { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Must call base.OnModelCreating to configure Identity tables
            base.OnModelCreating(modelBuilder);

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
        }
    }
}