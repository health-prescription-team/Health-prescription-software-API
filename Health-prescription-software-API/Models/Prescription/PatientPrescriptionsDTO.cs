namespace Health_prescription_software_API.Models.Prescription
{
    public class PatientPrescriptionsDTO
    {
        public PatientPrescriptionsDTO()
        {
            this.PatientPrescriptions = new HashSet<PatientPrescriptionsListDTO>();
        }

        public string PatientNames { get; set; } = null!;

        public string PatientEGN { get; set; } = null!;

        public byte[]? ProfileImage { get; set; }

        public IEnumerable<PatientPrescriptionsListDTO> PatientPrescriptions { get; set; }
    }
}
