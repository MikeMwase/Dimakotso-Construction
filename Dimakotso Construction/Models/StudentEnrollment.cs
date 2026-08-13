using Dimakotso_Construction.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dimakotso_Construction.Models
{
    public class StudentEnrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tracking Reference")]
        public string RegistrationNumber { get; set; } = $"DC-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name(s)")]
        public string FirstNames { get; set; }

        public string? MiddleNames { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13)]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Must be a valid 13-digit South African ID or Passport designation.")]
        [Display(Name = "ID / Passport Number")]
        public string IdentificationNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        // --- QCTO Core Demographic Compliance Elements ---
        [Required]
        public EquityCode Equity { get; set; }

        [Required]
        public CitizenStatusCode Citizenship { get; set; }

        [Required]
        [Display(Name = "Disability Status")]
        public DisabilityCode DisabilityStatus { get; set; }

        [Required]
        [Display(Name = "Current Employment Status")]
        public EmploymentStatus CurrentEmployment { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Highest Academic Qualification Passed")]
        public string HighestQualification { get; set; }

        // --- Contact Framework ---
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }


        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        public string City { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        // --- Academic Target Vectors ---
        //[Required]
        //[Display(Name = "Occupational Program Title")]
        //public string TargetProgramTitle { get; set; }

        //[Required]
        //[Display(Name = "SAQA Qualification ID")]
        //public int SaqaId { get; set; }

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Registered;

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // --- POPIA Compliance Tracking ---
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "POPIA operational data declaration acceptance is mandatory for QCTO registry data integration.")]
        public bool HasConsentedToPopiaDataSharing { get; set; }

        // --- Relational Properties ---
        [Display(Name = "Employer")]
        public int? EmployerId { get; set; }

        [ForeignKey(nameof(EmployerId))]
        [ValidateNever]
        public virtual Employer? Employer { get; set; }

        [ValidateNever]
        public virtual ComplianceDocument? VerificationDocuments { get; set; }

        // Many-to-Many relationship with Courses
        [ValidateNever]
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        [ValidateNever]
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }

    public class WorkplacePlacement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Host Employer Entity")]
        public string HostEmployerName { get; set; }

        [Required]
        public string CompanyVatNumber { get; set; }

        [Required]
        public string PlacementPhysicalAddress { get; set; }

        [Required]
        public string Province { get; set; }

        // --- Active Mentorship Constraints ---
        [Required]
        [Display(Name = "Assigned Trade Mentor")]
        public string MentorName { get; set; }

        [Required]
        public string MentorJobTitle { get; set; }

        [Required]
        [EmailAddress]
        public string MentorEmail { get; set; }

        [Required]
        [Phone]
        public string MentorCell { get; set; }
    }

    public class ComplianceDocument
    {
        [Key]
        [ForeignKey("StudentEnrollment")]
        public int StudentEnrollmentId { get; set; }

        [Required]
        public string CertifiedIdPath { get; set; }

        [Required]
        public string HighestQualificationCertificatePath { get; set; }

        public string SignedWorkplaceAgreementPath { get; set; }

        public virtual StudentEnrollment StudentEnrollment { get; set; }
    }
}