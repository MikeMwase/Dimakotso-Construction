using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dimakotso_Construction.Models
{
    public class AssessorQualification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AssessorId { get; set; }

        [ForeignKey(nameof(AssessorId))]
        [ValidateNever]
        public virtual Assessors Assessor { get; set; } = null!;

        [Required]
        [Display(Name = "Qualification Name")]
        public string QualificationName { get; set; } = string.Empty;

        [Display(Name = "Issuing Body")]
        public string? IssuingBody { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Obtained")]
        public DateTime? DateObtained { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        // Binary file storage
        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string ContentType { get; set; } = string.Empty;

        public long FileSizeBytes { get; set; }

        [Required]
        public byte[] FileData { get; set; } = Array.Empty<byte>();

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}