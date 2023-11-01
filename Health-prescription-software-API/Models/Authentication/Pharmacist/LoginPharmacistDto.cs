namespace Health_prescription_software_API.Models.Authentication.Pharmacist
{
    using System.ComponentModel.DataAnnotations;

    public class LoginPharmacistDto
    {
        [Required]
        public string Egn { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
