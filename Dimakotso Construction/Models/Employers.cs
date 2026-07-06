using System.ComponentModel.DataAnnotations;

namespace Dimakotso_Construction.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Person")]
        public string? ContactPerson { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string? ContactEmail { get; set; }

        [Display(Name = "Office/Site Phone")]
        [Phone]
        public string? ContactPhone { get; set; }

        [Display(Name = "Industry")]
        public string? Sector { get; set; }

        public string? Size { get; set; }

       // [Display(Name = "Physical Address")]
        public string? City { get; set; }

        public string? Province { get; set; }

       // [Display(Name = "Notes")]
        public string? Status { get; set; } 
    }
}