using Dimakotso_Construction.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dimakotso_Construction.Models
{
    public class StudentCourse
    {
        public int StudentEnrollmentId { get; set; }

        [ForeignKey(nameof(StudentEnrollmentId))]
        [ValidateNever]
        public StudentEnrollment StudentEnrollment { get; set; }

        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [ValidateNever]
        public Course Course { get; set; }

        // Extra information about this enrolment
        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        public EnrollmentStatus Status { get; set; }
            = EnrollmentStatus.Registered;

        public decimal? FinalMark { get; set; }

        public bool Competent { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string? CertificateNumber { get; set; }
    }
}