namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;
    using static Common.EntityValidationConstants.Prescription;
    using static Common.EntityValidationErrorMessages.Prescription;

    public class EditPrescriptionDTO
    {
        public EditPrescriptionDTO()
        {
            PrescriptionDetails = new HashSet<EditPrescriptionDetailsDTO>();
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string PatientEgn { get; set; } = null!;

        [Required]
        [Range(MinAge, MaxAge, ErrorMessage = InvalidAgeRangeMessage)]
        public int Age { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }

        public ICollection<EditPrescriptionDetailsDTO> PrescriptionDetails { get; set; }
    }
}
