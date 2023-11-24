namespace Health_prescription_software_API.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class PrescriptionDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int EveningDose {  get; set; }

        [Required]
        public int LunchTimeDose { get; set; }

        [Required]
        public int MorningDose { get; set; }

        [Required]
        public string MeasurementUnit { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
