namespace Health_prescription_software_API.Contracts
{
    using Health_prescription_software_API.Models.Prescription;

    public interface IPrescriptionService
    {
        Task<string> Add(AddPrescriptionDto prescriptionModel, string GpId);
    }
}