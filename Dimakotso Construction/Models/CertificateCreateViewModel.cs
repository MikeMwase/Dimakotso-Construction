using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dimakotso_Construction.Models
{
    public class CertificateCreateViewModel
    {
        public Certificate Certificate { get; set; } = new Certificate();

        public string LearnerFirstName { get; set; } = string.Empty;
        public string? LearnerMiddleNames { get; set; }
        public string LearnerSurname { get; set; } = string.Empty;
        public string LearnerEmail { get; set; } = string.Empty;
        public string LearnerMobileNumber { get; set; } = string.Empty;
        public string LearnerHomeAddress { get; set; } = string.Empty;
        public string LearnerCity { get; set; } = string.Empty;
        public string LearnerPostalCode { get; set; } = string.Empty;
        public string LearnerIdentificationNumber { get; set; } = string.Empty;

        public bool HasEmployer { get; set; }
        public string? EmployerCompanyName { get; set; }
        public string? EmployerContactPerson { get; set; }
        public string? EmployerContactEmail { get; set; }
        public string? EmployerContactPhone { get; set; }
        public string? EmployerCity { get; set; }
        public string? EmployerProvince { get; set; }

        // Selected course details — populated server-side, rendered directly (no AJAX needed for initial load)
        public string? SelectedCourseName { get; set; }
        public string? SelectedCourseCode { get; set; }
        public int? SelectedCourseNqfLevel { get; set; }
        public string? SelectedCourseDescription { get; set; }
        public int? SelectedCourseCredits { get; set; }
        public int? SelectedCourseDurationHours { get; set; }
        public string? SelectedCourseUnitStandardNumber { get; set; }

        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Assessors { get; set; } = new List<SelectListItem>();

        public bool RequiresStudentSelection { get; set; }
        public string? ErrorMessage { get; set; }
        public string CourseInfoJson { get; set; } = "[]";
    }
}