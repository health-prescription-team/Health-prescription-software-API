namespace Health_prescription_software_API.Models.Authentication.Pharmacist
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterPharmacistDto
    {
        [Required]
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public int Egn { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public IFormFile ProfilePicture { get; set; } = null!;

        [Required]
        public int UinNumber { get; set; }

        [Required]
        public string PharmacyName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;
    }
}
