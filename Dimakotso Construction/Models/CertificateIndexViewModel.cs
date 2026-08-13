using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dimakotso_Construction.Models
{
    public class CertificateIndexViewModel
    {
        public IEnumerable<Certificate> Certificates { get; set; } = new List<Certificate>();

        public string? SearchTerm { get; set; }
        public CertificateTrainingType? Type { get; set; }
        public CertificateStatus? Status { get; set; }
        public int? CourseId { get; set; }

        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();

        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalFilteredCertificates { get; set; }

        public int TotalCertificates { get; set; }
        public int ActiveCount { get; set; }
        public int ExpiringSoonCount { get; set; }
        public int ExpiredCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}