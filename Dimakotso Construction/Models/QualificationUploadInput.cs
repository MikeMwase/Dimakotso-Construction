using Microsoft.AspNetCore.Http;
using System;

namespace Dimakotso_Construction.Models
{
    public class QualificationUploadInput
    {
        public string QualificationName { get; set; } = string.Empty;
        public string? IssuingBody { get; set; }
        public DateTime? DateObtained { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public IFormFile? File { get; set; }
    }
}