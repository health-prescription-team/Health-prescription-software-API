namespace Health_prescription_software_API.Services
{
	using Health_prescription_software_API.Contracts;
	using Health_prescription_software_API.Data;
	using Health_prescription_software_API.Models.Medicine;
	using Microsoft.EntityFrameworkCore;
	using static Common.GeneralConstants;
	using static Common.EntityValidationErrorMessages.Medicine;

	public class ValidationMedicine : IValidationMedicine
	{
		private readonly HealthPrescriptionDbContext dbContext;

		private string? errorMessage;
		private string? errorPropName;
		public ValidationMedicine(HealthPrescriptionDbContext dbContext)
		{
			this.dbContext = dbContext;

			this.errorMessage = null;
			this.errorPropName = null;
		}

		public string? ErrorPropName { get => errorPropName; set => errorPropName = value; }
		public string? ErrorMessage { get => errorMessage; set => errorMessage = value; }

		public async Task<bool> IsQueryValid(QueryMedicineDTO? queryModel)
		{
			if (queryModel is null)
			{
				return true;
			}

			int entriesPerPage = DefaultHitsPerPage;
			if (queryModel.EntriesPerPage is not null)
			{
				entriesPerPage = (int)queryModel.EntriesPerPage;
			}

			int pageNumber = DefaultCurrentPage;
			if (queryModel.PageNumber is not null)
			{
				pageNumber = (int)queryModel.PageNumber;
			}

			int requiredEntries = entriesPerPage * pageNumber - entriesPerPage + 1;

			int availableEntries = await dbContext.Medicines
				.CountAsync(m => !m.IsDeleted);

			if (requiredEntries > availableEntries)
			{
				ErrorPropName = $"{nameof(queryModel.EntriesPerPage)}-{nameof(queryModel.PageNumber)}";
				ErrorMessage = InvalidQueryString;
				return false;
			}

			return true;
		}
	}
}
