using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentification.Patient
{
    public class PatientDto
    {
        [Required]
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int Egn { get; set; }
        [Required]
        public IFormFile ProfilePicture { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
