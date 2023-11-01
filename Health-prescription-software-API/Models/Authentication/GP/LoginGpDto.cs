using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentication.GP
{
    public class LoginGpDto
    {
        [Required]
        public string Egn {  get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
