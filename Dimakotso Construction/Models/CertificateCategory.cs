using System.ComponentModel.DataAnnotations;

namespace Dimakotso_Construction.Models
{
    /// <summary>
    /// Represents a certificate category
    /// </summary>
    public class CertificateCategory
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        [Display(Name = "Category Code")]
        public string? Code { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Sector")]
        public string? Sector { get; set; } // e.g., CETA, TETA

        [Display(Name = "Has Upload")]
        public bool HasUpload { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        public CertificateCategory? ParentCategory { get; set; }

        public List<CertificateCategory>? SubCategories { get; set; }
    }
}
