namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.User;

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
        public int Age { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }

        public ICollection<EditPrescriptionDetailsDTO> PrescriptionDetails { get; set; }
    }
}
