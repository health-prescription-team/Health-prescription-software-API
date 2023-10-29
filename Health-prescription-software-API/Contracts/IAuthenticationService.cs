using Health_prescription_software_API.Models.Authentification;
using Microsoft.IdentityModel.Tokens;

namespace Health_prescription_software_API.Contracts
{
    public interface IAuthenticationService
    {
        public Task<string> RegisterDoctor(GPDto model);
    }
}
