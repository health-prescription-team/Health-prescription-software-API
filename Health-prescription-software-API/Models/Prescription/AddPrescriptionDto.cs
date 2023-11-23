namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;

    public class AddPrescriptionDto
    {
        public AddPrescriptionDto()
        {
            this.PrescriptionDetails = new HashSet<PrescriptionDetailsDto>();
        }

        [Required]
        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string PatientEgn { get; set; } = null!;

        [Required]
        [Range(0, 200, ErrorMessage = "Invalid age.")]
        public int Age { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }

        public ICollection<PrescriptionDetailsDto> PrescriptionDetails { get; set; }
    }
}
