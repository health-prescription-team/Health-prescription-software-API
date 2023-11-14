namespace Health_prescription_software_API.Contracts.Validations
{
	using Health_prescription_software_API.Models.Authentication.Pharmacy;

	public interface IValidationAuthentication : IValidationErrorMessage
    {
        public Task<bool> IsPharmacyRegisterValid(RegisterPharmacyDto registerModel);
    }
}
