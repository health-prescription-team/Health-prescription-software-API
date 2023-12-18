namespace Health_prescription_software_API.Models.Prescription
{
    using System.ComponentModel.DataAnnotations;

    public class PrescriptionDetailsDTO
    {
        public Guid MedicineId { get; set; }

        public string MedicineName { get; set; } = null!;

        public string? Notes { get; set; }

        public int EveningDose { get; set; }

        public int LunchTimeDose { get; set; }

        public int MorningDose { get; set; }

        public string MeasurementUnit { get; set; } = null!;
    }
}
