namespace Health_prescription_software_API.Models.Authentication.Pharmacist
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;

    public class LoginPharmacistDto
    {
        [Required]
        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string Egn { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
