using System.ComponentModel.DataAnnotations;

namespace Dimakotso_Construction.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        #region Personal Information
        //1
        [StringLength(50)]
        public string Title { get; set; }

        //2
        [Required(ErrorMessage = "Surname is required.")]
        [Display(Name = "Surname")]
        [StringLength(100)]
        public string Surname { get; set; }

        //3
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        //4
        [Display(Name = "Middle Name")]
        [StringLength(100)]
        public string MiddleName { get; set; }

        //5
        [StringLength(10)]
        public string Initials { get; set; }
        #endregion

        #region Identity Information
        //6
        [Display(Name = "ID Number")]
        [StringLength(20)]
        public string IDNumber { get; set; }

        //7
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        //8
        [StringLength(20)]
        public string Gender { get; set; }

        //9
        [Display(Name = "Alternate ID Type")]
        [StringLength(50)]
        public string AlternateIDType { get; set; }

        //10
        [Display(Name = "Alternate ID Number")]
        [StringLength(50)]
        public string AlternateIDNumber { get; set; }
        #endregion

        //11
        #region Demographic Information
        [StringLength(50)]
        public string Ethnicity { get; set; }

        //12
        [StringLength(50)]
        public string Nationality { get; set; }

        //13
        [Display(Name = "Home Language")]
        [StringLength(50)]
        public string HomeLanguage { get; set; }
        #endregion

        //14
        #region Education & SDL Information
        [Display(Name = "Highest Education")]
        [StringLength(100)]
        public string HighestEducation { get; set; }

        //15
        [Display(Name = "Provider SDL Number")]
        [StringLength(50)]
        public string ProviderSDLNumber { get; set; }

        //16
        [Display(Name = "Employer SDL Number")]
        [StringLength(50)]
        public string EmployerSDLNumber { get; set; }
        #endregion

        //17
        #region Contact Information
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address (Optional)")]
        [StringLength(256)]
        public string EmailAddress { get; set; }

        //18
        [Display(Name = "Tel No")]
        [StringLength(20)]
        public string TelNo { get; set; }

        //19
        [Display(Name = "Fax No")]
        [StringLength(20)]
        public string FaxNo { get; set; }
        #endregion

        //20
        #region Address Information
        [StringLength(255)]
        public string Address { get; set; }

        //21
        [StringLength(100)]
        public string City { get; set; }

        //22
        [Display(Name = "State/Province")]
        [StringLength(100)]
        public string StateProvince { get; set; }

        //23
        [Display(Name = "Zip Code")]
        [StringLength(10)]
        public string ZipCode { get; set; }

        //24
        [StringLength(100)]
        public string Country { get; set; }
        #endregion

        //25
        #region Residence Information
        [Display(Name = "Citizen Resident Status")]
        [StringLength(100)]
        public string CitizenResidentStatus { get; set; }

        //26
        [Display(Name = "Geographical Area")]
        [StringLength(100)]
        public string GeographicalArea { get; set; }
        #endregion

        //27
        #region Additional Information
        [Display(Name = "Disability Status")]
        [StringLength(100)]
        public string DisabilityStatus { get; set; }
        
        //28
        [Display(Name = "Preferred Communication")]
        [StringLength(50)]
        public string PreferredCommunication { get; set; }

        //29
        [DataType(DataType.Date)]
        [Display(Name = "Date Obtained")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateObtained { get; set; }

        //30
        [Display(Name = "Socio Economic Status")]
        [StringLength(100)]
        public string SocioEconomicStatus { get; set; }
        #endregion

        //31
        #region Employment Information
        [Required(ErrorMessage = "Employer selection is required.")]
        [Display(Name = "Employer")]
        public string Employer { get; set; } // Map to int EmployerId if using an explicit entity relationship

        //32
        [Display(Name = "Job Title")]
        [StringLength(100)]
        public string JobTitle { get; set; }
        #endregion

        //33
        #region System Information
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";
        #endregion
    }
}
