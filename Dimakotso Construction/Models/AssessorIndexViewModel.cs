using System.Collections.Generic;

namespace Dimakotso_Construction.Models
{
    public class AssessorIndexViewModel
    {
        public IEnumerable<Assessors> Assessors { get; set; } = new List<Assessors>();

        public string? SearchTerm { get; set; }
        public string? Department { get; set; }
        public string? Status { get; set; }
        public List<string> Departments { get; set; } = new List<string>();

        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalFilteredAssessors { get; set; }

        public int TotalAssessors { get; set; }
        public int ActiveCount { get; set; }
        public int ExpiringCount { get; set; }
        public int InactiveCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}