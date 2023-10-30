using Health_prescription_software_API.Models.Authentication.GP;
using Health_prescription_software_API.Models.Authentication.Pharmacy;
using Microsoft.IdentityModel.Tokens;

namespace Health_prescription_software_API.Contracts
{
    public interface IAuthenticationService
    {
        public Task<string> RegisterGp(RegisterGpDto model);

        public Task<string> LoginGp(LoginGpDto model);
		public Task<string> RegisterPharmacy(RegisterPharmacyDto pharmacyUser);
	}
}
