using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities;

namespace Health_prescription_software_API.Services
{
    public class MedicineService:IMedicineService
    {
        private readonly HealthPrescrtionDbContext _context;

        public MedicineService(HealthPrescrtionDbContext context)
        {
            _context = context;
        }

        public async Task<Medicine?> GetById(int id)
        {
            return await _context.Medicines.FindAsync(id);
        }
    }
}
