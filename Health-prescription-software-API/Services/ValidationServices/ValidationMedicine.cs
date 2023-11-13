namespace Health_prescription_software_API.Services.ValidationServices
{
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Models.Medicine;
    using Microsoft.EntityFrameworkCore;
    using static Common.GeneralConstants;
    using static Common.EntityValidationErrorMessages.Medicine;
    using Health_prescription_software_API.Contracts.Validations;

    public class ValidationMedicine : IValidationMedicine
    {
        private readonly HealthPrescriptionDbContext dbContext;

        public ValidationMedicine(HealthPrescriptionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string? ErrorPropName { get; set; }
        public string? ErrorMessage { get; set; }

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
