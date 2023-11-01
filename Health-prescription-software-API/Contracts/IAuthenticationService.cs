using Health_prescription_software_API.Models.Authentication.GP;
using Health_prescription_software_API.Models.Authentication.Pharmacy;
using Health_prescription_software_API.Models.Authentication.Pharmacist;
using Microsoft.IdentityModel.Tokens;
using Health_prescription_software_API.Models.Authentication.Patient;

namespace Health_prescription_software_API.Contracts
{
    public interface IAuthenticationService
    {
        public Task<string?> LoginPatient(LoginPatientDto model);
        public Task<string?> RegisterPatient(PatientDto model);

        public Task<string?> RegisterGp(RegisterGpDto model);

        public Task<string?> LoginGp(LoginGpDto model);


		public Task<string?> RegisterPharmacy(RegisterPharmacyDto pharmacyUser);

		public Task<string?> LoginPharmacy(LoginPharmacyDto pharmacyUser);
	}

}
