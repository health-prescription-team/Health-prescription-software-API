using Health_prescription_software_API.Models.Prescription;

namespace Health_prescription_software_API.Contracts.Validations
{
    public interface IValidationPrescription: IValidationErrorMessage
    {
        Task<bool> IsAddPrescriptionValid(AddPrescriptionDto prescriptionModel);

        Task<bool> IsPatientPrescriptionsValid(string patientEgn);
    }
}
