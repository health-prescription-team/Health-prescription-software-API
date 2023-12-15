namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;
    using static Common.EntityValidationConstants.Prescription;
    using static Common.EntityValidationErrorMessages.Prescription;

    public class AddPrescriptionDto
    {
        public AddPrescriptionDto()
        {
            this.PrescriptionDetails = new HashSet<AddPrescriptionDetailsDto>();
        }

        [Required]
        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string PatientEgn { get; set; } = null!;

        [Required]
        [Range(MinAge, MaxAge, ErrorMessage = InvalidAgeRangeMessage)]
        public int Age { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }

        public ICollection<AddPrescriptionDetailsDto> PrescriptionDetails { get; set; }
    }
}
