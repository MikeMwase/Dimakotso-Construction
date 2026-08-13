using System.Collections.Generic;

namespace Dimakotso_Construction.Models
{
    public class CourseIndexViewModel
    {
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();

        public string? SearchTerm { get; set; }

        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalFilteredCourses { get; set; }

        // Stats always reflect the full unfiltered dataset
        public int TotalCourses { get; set; }
        public int ActiveCourses { get; set; }
        public int TotalEnrolled { get; set; }
        public int TotalHours { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}