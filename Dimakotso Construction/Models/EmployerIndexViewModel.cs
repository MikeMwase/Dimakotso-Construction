using System.Collections.Generic;

namespace Dimakotso_Construction.Models
{
    public class EmployerIndexViewModel
    {
        public IEnumerable<Employer> Employers { get; set; } = new List<Employer>();

        public string? SearchTerm { get; set; }
        public string? Sector { get; set; }
        public string? Province { get; set; }
        public string? Status { get; set; }

        public List<string> Sectors { get; set; } = new List<string>();
        public List<string> Provinces { get; set; } = new List<string>();
        public List<string> Statuses { get; set; } = new List<string>();

        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalFilteredEmployers { get; set; }

        public int TotalEmployers { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}