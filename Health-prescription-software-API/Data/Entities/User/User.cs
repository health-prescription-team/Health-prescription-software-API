namespace Health_prescription_software_API.Data.Entities.User
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;

    [Index(nameof(Egn), IsUnique = true)]
    [Index(nameof(UinNumber), IsUnique = true)]
    public class User : IdentityUser
    {
        public User() : base()
        {
            this.PharmacyMedicines = new HashSet<Medicine>();
        }

        [MaxLength(NameMaxLength)]
        public string? FirstName { get; set; } = null!;

        [MaxLength(NameMaxLength)]
        public string? MiddleName { get; set; }

        [MaxLength(NameMaxLength)]
        public string? LastName { get; set; }

        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string? Egn { get; set; }

        public byte[]? ProfilePicture { get; set; }

        [RegularExpression(UinRegexPattern, ErrorMessage = InvalidUniErrorMessage)]
        public string? UinNumber { get; set; }

        [MaxLength(HospitalNameMaxLength)]
        public string? HospitalName { get; set; }

        [MaxLength(PharmacyNameMaxLength)]
        public string? PharmacyName { get; set; }

        [Phone]
        [RegularExpression(PhoneNumberRegexPattern, ErrorMessage = InvalidPhoneNumberErrorMessage)]
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public virtual ICollection<Medicine> PharmacyMedicines { get; set; }
    }
}