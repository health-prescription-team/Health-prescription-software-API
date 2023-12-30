namespace Health_prescription_software_API.Services
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities;
    using Health_prescription_software_API.Models.Medicine;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using static Common.GeneralConstants;


    public class MedicineService : IMedicineService
    {
        private readonly HealthPrescriptionDbContext context;

        public MedicineService(HealthPrescriptionDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Add(AddMedicineDTO model, string creatorId)
        {
            using var memoryStream = new MemoryStream();
            await model.MedicineImage.CopyToAsync(memoryStream);

            var medicine = new Medicine
            {
                Name = model.Name,
                Price = model.Price,
                MedicineImageBytes = memoryStream.ToArray(),
                MedicineDetails = model.MedicineDetails,
                MedicineCompany = model.MedicineCompany,
                Ingredients = model.Ingredients,
                OwnerId = creatorId
            };

            await context.Medicines.AddAsync(medicine);
            await context.SaveChangesAsync();

            return medicine.Id;
        }

        public async Task<MedicineDetailsDTO> GetById(Guid id)
        {
            var medicine = await context.Medicines.Include(m => m.Owner).FirstOrDefaultAsync(m => m.Id == id);

            var medicineDTO = new MedicineDetailsDTO
            {
                Name = medicine!.Name,
                MedicineImageBytes = medicine.MedicineImageBytes,
                Price = medicine.Price,
                MedicineCompany = medicine.MedicineCompany,
                MedicineDetails = medicine.MedicineDetails,
                Ingredients = medicine.Ingredients,
                PharmacyName = medicine.Owner.PharmacyName!,
                PharmacyId = medicine.Owner.Id
            };

            return medicineDTO;
        }


        //todo: Edit must be performed only by noTech admin. 
        //todo: Edit and Add must be introduced to Pharmacy as well. It ought to  edit price to the medicine.
        public async Task EditByIdAsync(Guid id, EditMedicineDTO editMedicineModel)
        {

            Medicine medicineToEdit = await this.context.Medicines.FirstAsync(m => m.Id == id);
            medicineToEdit.Name = editMedicineModel.Name;
            //medicineToEdit.AveragePrice = editMedicineModel.Price;
            medicineToEdit.MedicineCompany = editMedicineModel.MedicineCompany;
            medicineToEdit.MedicineDetails = editMedicineModel.MedicineDetails;
            medicineToEdit.MedicineImageBytes = editMedicineModel.MedicineImageBytes;
            await this.context.SaveChangesAsync();
        }



        public async Task<AllMedicineServiceModel> GetAllAsync(QueryMedicineDTO? query)
        {
            IQueryable<Medicine> medicineQuery = context.Medicines
                .AsQueryable()
                .AsNoTracking();


            string? name = query?.SearchTerm;
            if (!string.IsNullOrEmpty(name))
            {
                medicineQuery = medicineQuery
                    .Where(m => m.Name.ToLower().Contains(name.ToLower()))
                    //todo: obtain ordering from query string if needed
                    .OrderBy(m => m.Name)
                    .ThenBy(m => m.MedicineCompany)
                    .ThenByDescending(m => m.Price);
            }

            int medicinesCount = medicineQuery.Count();

            if (query?.PageNumber != null && query?.PageNumber != 0)
            {
                int currentPage = query?.PageNumber ?? DefaultCurrentPage;
                int hitsPerPage = query?.EntriesPerPage ?? DefaultHitsPerPage;

                medicineQuery = medicineQuery
                    .Skip((currentPage - 1) * hitsPerPage)
                    .Take(hitsPerPage);
            }


            AllMedicineDTO[] medicines = await medicineQuery
                .Select(m => new AllMedicineDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    MedicineCompany = m.MedicineCompany,
                    Price = m.Price,
                    MedicineImageBytes = m.MedicineImageBytes
                })
                .ToArrayAsync();

            AllMedicineServiceModel model = new AllMedicineServiceModel()
            {
                Medicines = medicines,
                MedicinesCount = medicinesCount
            };

            return model;
        }

        public async Task<IEnumerable<AllMedicineMinimalDTO>> GetAllMinimalAsync()
        {
            var modelDb = await context.Medicines
                .AsNoTracking()
                .Select(x => new AllMedicineMinimalDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();

            return modelDb;
        }

        public async Task<bool> Delete(Guid id)
        {
            var medicine = await context.Medicines.FirstOrDefaultAsync(m => m.Id == id);

            if (medicine != null)
            {
                context.Medicines.Remove(medicine);

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}