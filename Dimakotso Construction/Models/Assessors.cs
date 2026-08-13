using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Dimakotso_Construction.Models
{
    public class Assessors
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Contact { get; set; }

        [Display(Name = "Assessor Number")]
        public string AssessorNumber { get; set; }

        public string Department { get; set; }
        public string Status { get; set; }

        [Display(Name = "Qualifications")]
        public string? qualifications { get; set; }

        public string Assessor 
        {
            get 
            { 
               return $"{FirstName} - {LastName}";
            }
        }

        [ValidateNever]
        public virtual ICollection<AssessorCourse> AssessorCourses { get; set; } = new List<AssessorCourse>();

        [ValidateNever]
        public virtual ICollection<AssessorQualification> QualificationDocuments { get; set; } = new List<AssessorQualification>();
    }
}
