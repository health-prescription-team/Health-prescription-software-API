using Health_prescription_software_API.Models.Prescription;

namespace Health_prescription_software_API.Contracts.Validations
{
    public interface IValidationPrescription: IValidationErrorMessage
    {
        Task<bool> IsAddPrescriptionValid(AddPrescriptionDto model);

        Task<bool> IsPatientPrescriptionsValid(string patientEgn);

        Task<bool> IsPrescriptionValid(Guid patientId);

        Task<bool> IsEditPrescriptionValid(EditPrescriptionDTO model);

        Task<bool> IsGpThePrescriber(string gpId, Guid prescriptionId);
    }
}
