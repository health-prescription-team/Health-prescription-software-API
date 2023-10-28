using Health_prescription_software_API.Contracts;

namespace Health_prescription_software_API.Controllers
{
    public class AuthenticationController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
    }
}
