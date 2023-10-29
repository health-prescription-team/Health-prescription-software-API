using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentification.GP
{
    public class LoginGpDto
    {
        [Required]
        public int Egn {  get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
