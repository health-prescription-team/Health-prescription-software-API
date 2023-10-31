namespace Health_prescription_software_API.Models.Authentification.Pharmacist
{
    using System.ComponentModel.DataAnnotations;

    public class LoginPharmacistDto
    {
        [Required]
        public int Egn { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
