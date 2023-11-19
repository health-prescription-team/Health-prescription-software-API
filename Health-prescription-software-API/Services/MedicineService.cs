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

		public async Task Add(AddMedicineDTO model)
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
					AveragePrice = model.Price,
					MedicineImageBytes = memoryStream.ToArray(),
					MedicineDetails = model.MedicineDetails,
					IsDeleted = false

				};

				await context.Medicines.AddAsync(modelDb);
				await context.SaveChangesAsync();
			}
		}
      
		public async Task<MedicineDetailsDTO?> GetById(int id)
		{
				var medicine = await context.Medicines.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

				if (medicine != null)
				{
					var medicineDTO = new MedicineDetailsDTO
					{
						Name = medicine.Name,
						MedicineImageBytes = medicine.MedicineImageBytes,
						Price = medicine.AveragePrice,
						MedicineCompany = medicine.MedicineCompany,
						MedicineDetails = medicine.MedicineDetails
					};

					return medicineDTO;
				}

				return null;
		 }


		public async Task EditByIdAsync(int id, EditMedicineDTO editMedicineModel)
		{

			Medicine medicineToEdit = await this.context.Medicines.FirstAsync(m => m.Id == id && !m.IsDeleted);
			medicineToEdit.Name = editMedicineModel.Name;
			medicineToEdit.AveragePrice = editMedicineModel.Price;
			medicineToEdit.MedicineCompany = editMedicineModel.MedicineCompany;
			medicineToEdit.MedicineDetails = editMedicineModel.MedicineDetails;
			medicineToEdit.MedicineImageBytes = editMedicineModel.MedicineImageBytes;
			await this.context.SaveChangesAsync();
		}



		public async Task<AllMedicineServiceModel> GetAllAsync(QueryMedicineDTO? query)
		{
			IQueryable<Medicine> medicineQuery = context.Medicines
				.AsQueryable()
				.Where(m => !m.IsDeleted)
				.AsNoTracking();


			string? name = query?.SearchTerm;
			if (!string.IsNullOrEmpty(name))
			{
				medicineQuery = medicineQuery
					.Where(m => m.Name.ToLower().Contains(name.ToLower()))
					//todo: obtain ordering from query string if needed
					.OrderBy(m => m.Name)
					.ThenBy(m => m.MedicineCompany)
					.ThenByDescending(m => m.AveragePrice);
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
					Price = m.AveragePrice,
					MedicineImageBytes  = m.MedicineImageBytes
				})
				.ToArrayAsync();

			AllMedicineServiceModel model = new AllMedicineServiceModel()
			{
				Medicines = medicines,
				MedicinesCount = medicinesCount
			};

			return model;
		}

        public async Task<bool> Delete(int id)
        {
            var medicine = await context.Medicines.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

			if (medicine != null)
			{
				medicine.IsDeleted = true;

				await context.SaveChangesAsync();

				return true;
			}

			return false;
        }
    }
}