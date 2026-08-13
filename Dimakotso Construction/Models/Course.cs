using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Dimakotso_Construction.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Duration (Hours)")]
        public int DurationHours { get; set; }

        [Required]
        public CourseStatus Status { get; set; }

        [Required]
        public CourseType Type { get; set; }

        [Required]
        public CertificationType Certification { get; set; }

        public string Prerequisites { get; set; } = string.Empty;
        public string LearningObjectives { get; set; } = string.Empty;

        [Range(1, 10)]
        [Display(Name = "NQF Level")]
        public int NQFLevel { get; set; }

        [Range(1, 300)]
        public int Credits { get; set; }

        [Display(Name = "Unit Standard Number")]
        public string? UnitStandardNumber { get; set; }

        [ValidateNever]
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        [ValidateNever]
        public virtual ICollection<AssessorCourse> AssessorCourses { get; set; } = new List<AssessorCourse>();
    }

    public enum CourseStatus
    {
        Active,
        Inactive,
        Archived
    }

    public enum CertificationType
    {
        CETA,
        TETA,
        MICTSETA,
        QCTO
    }

    public enum CourseType
    {
        Qualification,

        Learnership,

        [Display(Name = "Skills Programme")]
        SkillsProgramme,

        Apprenticeship,

        [Display(Name = "Short Course")]
        ShortCourse
    }
}