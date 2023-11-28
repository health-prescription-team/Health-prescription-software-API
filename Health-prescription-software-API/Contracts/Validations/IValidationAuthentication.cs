namespace Health_prescription_software_API.Contracts.Validations
{
    using Health_prescription_software_API.Models.Authentication.Pharmacist;
    using Health_prescription_software_API.Models.Authentication.Pharmacy;
    using Health_prescription_software_API.Models.Authentication.Patient;

	public interface IvalidationPrescription : IValidationErrorMessage
    {
        public Task<bool> IsPharmacyRegisterValid(RegisterPharmacyDto registerModel);

        Task<bool> IsPharmacistRegisterValid(RegisterPharmacistDto registerModel);

        Task<bool> IsPharmacistLoginValid(LoginPharmacistDto loginModel);

        Task<bool> IsPatientRegisterValid(PatientDto registerModel);

        Task<bool> IsPatientLoginValid(LoginPatientDto loginModel);
    }
}
