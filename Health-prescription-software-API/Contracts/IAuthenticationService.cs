using Health_prescription_software_API.Models.Authentification.GP;
using Health_prescription_software_API.Models.Authentification.Pharmacist;

namespace Health_prescription_software_API.Contracts
{
    public interface IAuthenticationService
    {
        public Task<string> RegisterGp(RegisterGpDto model);

        public Task<string> LoginGp(LoginGpDto model);

        public Task<string?> RegisterPharmacist(RegisterPharmacistDto model);

        public Task<string> LoginPharmacist(LoginPharmacistDto model);
    }
}
