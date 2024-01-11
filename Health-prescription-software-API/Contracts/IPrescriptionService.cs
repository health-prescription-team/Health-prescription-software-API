namespace Health_prescription_software_API.Contracts
{
    using Health_prescription_software_API.Models.Prescription;

    public interface IPrescriptionService
    {
        Task<Guid> Add(AddPrescriptionDto prescriptionModel, string GpId);

        Task<PatientPrescriptionsDTO> GetPatientPrescriptions(string patientEgn);

        Task<PrescriptionDTO> GetPrescriptionDetails(Guid prescriptionId);

        void Delete(Guid id);

        Task<Guid> Edit(EditPrescriptionDTO prescriptionModel, string GpId);

        Task<bool> FinishPrescription(Guid id);
    }
}