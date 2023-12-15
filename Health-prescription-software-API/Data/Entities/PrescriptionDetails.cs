namespace Health_prescription_software_API.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PrescriptionDetails
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PrescriptionId { get; set; }
        [ForeignKey(nameof(PrescriptionId))]
        public virtual Prescription Prescription { get; set; } = null!;

        [Required]
        public Guid MedicineId { get; set; }
        [ForeignKey(nameof(MedicineId))]
        public virtual Medicine Medicine { get; set; } = null!;

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
