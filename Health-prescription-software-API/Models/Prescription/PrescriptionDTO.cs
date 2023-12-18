namespace Health_prescription_software_API.Models.Prescription
{
    public class PrescriptionDTO
    {
        public PrescriptionDTO()
        {
            this.PrescriptionDetails = new HashSet<PrescriptionDetailsDTO>();
        }

        public Guid Id { get; set; }

        public string GpId { get; set; } = null!;

        public string PatientEgn { get; set; } = null!;

        public int Age { get; set; }

        public string Diagnosis { get; set; } = null!;

        public bool IsFulfilled { get; set; }

        public string? FulfillmentDate { get; set; }

        public string CreatedAt { get; set; } = null!;

        public string? ExpiresAt { get; set; }

        public IEnumerable<PrescriptionDetailsDTO> PrescriptionDetails { get; set; }
    }
}
