using Health_prescription_software_API.Models.Prescription;

namespace Health_prescription_software_API.Contracts
{
    public interface IPrescriptionService
    {
        Task<int> Add(AddPrescriptionDto prescriptionModel);
    }
}
