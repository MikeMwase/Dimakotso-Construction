using System.ComponentModel.DataAnnotations;

namespace Dimakotso_Construction.Models
{
    public class Assessors
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string EmployeeId { get; set; }

        public string Department { get; set; }
        public string Status { get; set; }

        public string qualifications { get; set; }

        public string Assessor 
        {
            get 
            { 
               return $"{FirstName} - {LastName}";
            }
        }
    }
}
