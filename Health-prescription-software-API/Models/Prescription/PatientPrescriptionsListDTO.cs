namespace Health_prescription_software_API.Models.Prescription
{
    public class PatientPrescriptionsListDTO
    {
        public Guid PrescriptionId { get; set; }

        public string PatientNames { get; set; } = null!;

        public string PatientEGN { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public string? ExpiresAt { get; set; }

        public bool IsFulfilled { get; set; }

        public IEnumerable<string> Medicaments { get; set; } = Enumerable.Empty<string>();
    }
}
