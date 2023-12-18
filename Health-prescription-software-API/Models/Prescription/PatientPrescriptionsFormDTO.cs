namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    public class PatientPrescriptionsFormDTO
    {
        [Required]
        public string EGN { get; set; } = null!;
    }
}
