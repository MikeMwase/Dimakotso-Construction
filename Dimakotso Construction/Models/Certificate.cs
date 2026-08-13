using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dimakotso_Construction.Models
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }

        #region Certificate Core Info
        [Display(Name = "Certificate Number")]
        public string? CertificateNumber { get; set; } // Auto-generated string

        [Required]
        [Display(Name = "Job Number")]
        public string JobNumber { get; set; } = string.Empty;

        [Required]
        public CertificateTrainingType Type { get; set; } = CertificateTrainingType.Novice;

        [Required]
        [Display(Name = "Certificate Type")]
        public string CertificateCategory { get; set; } = string.Empty; // e.g., "Health & Safety - CETA", "Lifting Equipment - TETA"

        public CertificateStatus Status { get; set; } = CertificateStatus.Active;

        [Display(Name = "Certificate Title")]
        public string? CertificateTitle { get; set; }

        [Display(Name = "Certificate Level")]
        public string? CertificateLevel { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Certificate Description")]
        public string? CertificateDescription { get; set; }
        #endregion

        #region Relationships (Normalized)
        // 1. Learner Reference (Replaces duplicate student fields)
        [Required]
        [Display(Name = "Learner")]
        public int StudentEnrollmentId { get; set; }

        [ForeignKey(nameof(StudentEnrollmentId))]
        [ValidateNever]
        public virtual StudentEnrollment StudentEnrollment { get; set; } = null!;

        // 2. Course Reference (Replaces duplicate CourseCode, CourseName, Credits, NQFLevel)
        [Required]
        [Display(Name = "Course Title")]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [ValidateNever]
        public virtual Course Course { get; set; } = null!;

        // 3. Assessor Reference
        [Display(Name = "Assessor")]
        public int? AssessorId { get; set; }

        [ForeignKey(nameof(AssessorId))]
        [ValidateNever]
        public virtual Assessors? Assessor { get; set; }

        [Display(Name = "Assessor Qualification")]
        public string? SelectedAssessorQualification { get; set; }
        #endregion

        #region Machine / Plant Equipment Information
        [Display(Name = "Machine Code")]
        public string? MachineCode { get; set; }

        [Display(Name = "Machine Description")]
        public string? MachineDescription { get; set; }

        [Display(Name = "Machine Capacity")]
        public string? MachineCapacity { get; set; }

        [Display(Name = "Machine Model")]
        public string? MachineModel { get; set; }

        // M-to-M Collections for dynamic checkboxes
        public virtual ICollection<CertificateAttachment> CertificateAttachments { get; set; } = new List<CertificateAttachment>();
        public virtual ICollection<CertificateRestriction> CertificateRestrictions { get; set; } = new List<CertificateRestriction>();
        #endregion

        #region License Validity
        [Display(Name = "Needs Fitness Certificate")]
        public bool NeedsFitnessCertificate { get; set; } = false;

        [Display(Name = "No Expiry / Advise Renew")]
        public bool NoExpiryAdviseRenew { get; set; } = false;
        #endregion

        #region Important Dates
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }
        #endregion

        #region Assessment Details & Scores
        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        [Display(Name = "Theoretical Percentage")]
        public decimal? TheoreticalPercentage { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        [Display(Name = "Practical Percentage")]
        public decimal? PracticalPercentage { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        [Display(Name = "Final Percentage")]
        public decimal? FinalPercentage { get; set; }

        [DataType(DataType.MultilineText)]
        public string? AssessmentMetadata { get; set; }

        [DataType(DataType.MultilineText)]
        public string? VerificationAndCompliance { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
        #endregion
    }

    public enum CertificateTrainingType
    {
        Novice,
        Refresher,
        ReCertification
    }

    public enum CertificateStatus
    {
        Active,
        Expired,
        Revoked,
        Pending
    }
}