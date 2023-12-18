namespace Health_prescription_software_API.Contracts
{
    using Health_prescription_software_API.Models.Prescription;

    public interface IPrescriptionService
    {
        Task<Guid> Add(AddPrescriptionDto prescriptionModel, string GpId);

        Task<IEnumerable<PatientPrescriptionsListDTO>> GetPatientPrescriptions(string patientEgn);

        Task<PrescriptionDTO> GetPrescriptionDetails(Guid prescriptionId);
    }
}