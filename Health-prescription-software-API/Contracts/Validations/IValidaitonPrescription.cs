using Health_prescription_software_API.Models.Prescription;

namespace Health_prescription_software_API.Contracts.Validations
{
    public interface IValidaitonPrescription: IValidationErrorMessage
    {
        Task<bool> IsPrescriptionValid(AddPrescriptionDto prescriptionModel);

    }
}
