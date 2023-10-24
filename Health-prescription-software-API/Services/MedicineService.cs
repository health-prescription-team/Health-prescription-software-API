using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;

namespace Health_prescription_software_API.Services
{
    public class MedicineService:IMedicineService
    {
        private readonly HealthPrescrtionDbContext _context;

        public MedicineService(HealthPrescrtionDbContext context)
        {
            _context = context;
        }
    }
}
