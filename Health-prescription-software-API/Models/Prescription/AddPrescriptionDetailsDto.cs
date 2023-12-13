namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    public class AddPrescriptionDetailsDto
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int EveningDose { get; set; }

        [Required]
        public int LunchTimeDose { get; set; }

        [Required]
        public int MorningDose { get; set; }

        [Required]
        public string MeasurementUnit { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
