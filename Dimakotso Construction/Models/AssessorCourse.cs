using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dimakotso_Construction.Models
{
    public class AssessorCourse
    {
        public int AssessorId { get; set; }

        [ForeignKey(nameof(AssessorId))]
        [ValidateNever]
        public virtual Assessors Assessor { get; set; } = null!;

        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [ValidateNever]
        public virtual Course Course { get; set; } = null!;

        public string AccreditationNumber { get; set; } = string.Empty;
    }
}