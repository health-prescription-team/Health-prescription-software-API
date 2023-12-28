namespace Health_prescription_software_API.Models.Authentication.Patient
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;

    public class PatientDto
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; } = null!;

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone]
        [RegularExpression(PhoneNumberRegexPattern, ErrorMessage = InvalidPhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string Egn { get; set; } = null!;

        [Required]
        public IFormFile ProfilePicture { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

    }
}
