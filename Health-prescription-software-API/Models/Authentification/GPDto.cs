using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentification
{
    public class GPDto
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Egn { get; set; }
        [Required]
        public IFormFile ProfilePicture { get; set; }
        [Required]
        public int UinNumber { get; set; }

        [Required]
        public string HospitalName { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]
        public string PhonrNumber { get; set; }
    }
}
