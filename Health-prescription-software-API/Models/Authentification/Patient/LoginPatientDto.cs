using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentification.Patient
{
    public class LoginPatientDto
    {
        [Required]
        public int Egn { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
