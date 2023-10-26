
using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Health_prescription_software_API.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly HealthPrescriptionDbContext _context;

        public MedicineService(HealthPrescriptionDbContext context)
        {
            _context = context;
        }


        public async Task<MedicineDetailsDTO?> GetById(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine != null)
            {
                var medicineDTO = new MedicineDetailsDTO
                {
                    Name = medicine.Name,
                    MedicineImageBytes = medicine.MedicineImageBytes,
                    Price = medicine.Price,
                    MedicineCompany = medicine.MedicineCompany,
                    MedicineDetails = medicine.MedicineDetails
                };

                return medicineDTO;
            }

            return null;
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
     
      
        
        public async Task EditByIdAsync(int id, EditMedicineDTO editMedicineModel)
        {
            
            Medicine medicineToEdit = await this._context.Medicines.FirstAsync(m => m.Id == id);
            medicineToEdit.Name = editMedicineModel.Name;
            medicineToEdit.Price = editMedicineModel.Price;
            medicineToEdit.MedicineCompany = editMedicineModel.MedicineCompany;
            medicineToEdit.MedicineDetails = editMedicineModel.MedicineDetails;
            medicineToEdit.MedicineImageBytes = editMedicineModel.MedicineImageBytes;
            await this._context.SaveChangesAsync();
        }
    }
}
