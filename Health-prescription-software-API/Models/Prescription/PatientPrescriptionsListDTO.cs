namespace Health_prescription_software_API.Models.Prescription
{
    public class PatientPrescriptionsListDTO
    {
        public Guid PrescriptionId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public bool IsFulfilled { get; set; }

        public IEnumerable<string> Medicaments { get; set; } = Enumerable.Empty<string>();
    }
}
