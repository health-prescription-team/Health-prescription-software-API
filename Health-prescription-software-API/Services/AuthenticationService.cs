using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;

namespace Health_prescription_software_API.Services
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly HealthPrescriptionDbContext _context;

        public AuthenticationService(HealthPrescriptionDbContext context)
        {
            _context = context;
        }
    }
}
