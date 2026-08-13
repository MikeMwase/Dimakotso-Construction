using System.Collections.Generic;
using Dimakotso_Construction.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dimakotso_Construction.Models
{
    public class StudentEnrollmentIndexViewModel
    {
        public IEnumerable<StudentEnrollment> Students { get; set; } = new List<StudentEnrollment>();

        public string? SearchTerm { get; set; }
        public EnrollmentStatus? Status { get; set; }
        public int? EmployerId { get; set; }

        public List<SelectListItem> Employers { get; set; } = new List<SelectListItem>();

        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalFilteredStudents { get; set; }

        public int TotalStudents { get; set; }
        public int ActiveTrainingCount { get; set; }
        public int RegisteredCount { get; set; }
        public int OtherStatusCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}