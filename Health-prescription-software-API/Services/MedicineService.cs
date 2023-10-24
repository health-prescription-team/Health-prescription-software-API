
using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;


namespace Health_prescription_software_API.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly HealthPrescriptionDbContext _context;

        public MedicineService(HealthPrescriptionDbContext context)
        {
            _context = context;
        }


        public async Task<Medicine?> GetById(int id)
        {
            return await _context.Medicines.FindAsync(id);
        }

        public async void Add(AddMedicineDTO model)
        {
            if (model is null)
            {
                throw new NullReferenceException("Medicine model cannot be null!");
            }

            if (model.MedicineFile == null || model.MedicineFile.Length == 0)
            {
                throw new NullReferenceException("Image model cannot be null!");
            }

            using (var memoryStream = new MemoryStream())
            {
                await model.MedicineFile.CopyToAsync(memoryStream);

                var modelDb = new Medicine
                {
                    MedicineCompany = model.MedicineCompany,
                    Name = model.Name,
                    Price = model.Price,
                    MedicineImageBytes = memoryStream.ToArray(),
                    MedicineDetails = model.MedicineDetails

                };

                _context.Medicines.Add(modelDb);
                _context.SaveChanges();







            }

          


        }
    }
}
