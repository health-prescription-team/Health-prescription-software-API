namespace Health_prescription_software_API.Services.ValidationServices
{
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Models.Medicine;
    using Microsoft.EntityFrameworkCore;
    using static Common.GeneralConstants;
    using static Common.EntityValidationErrorMessages.Medicine;
    using Health_prescription_software_API.Contracts.Validations;
    using System.Collections.Generic;
    using System;

    public class ValidationMedicine : IValidationMedicine
    {
        private readonly HealthPrescriptionDbContext dbContext;

        public ValidationMedicine(HealthPrescriptionDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.ModelErrors = new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsMedicineValid(Guid id)
        {
            bool isIdValid = await dbContext.Medicines.AnyAsync(m => m.Id == id);

            if (!isIdValid)
            {
                var modelError = new ModelError {
                    ErrorPropName = "Id",
                    ErrorMessage = InvalidMedicineId
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        public async Task<bool> IsPharmacyMedicineOwner(string pharmacyId, Guid medicineId)
        {
            var medicine = await dbContext.Medicines.FindAsync(medicineId);

            return medicine?.OwnerId == pharmacyId;
        }

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
                .CountAsync();

            if (requiredEntries > availableEntries)
            {
                var errorPageNumber = new ModelError
                {
                    ErrorPropName = $"{nameof(queryModel.PageNumber)}",
                    ErrorMessage = InvalidQueryString
                }; 
                
                var errorEntriesPerPage = new ModelError
                {
                    ErrorPropName = $"{nameof(queryModel.EntriesPerPage)}",
                    ErrorMessage = InvalidQueryString
                };

                ModelErrors.Add(errorPageNumber);
                ModelErrors.Add(errorEntriesPerPage);

                return false;
            }

            return true;
        }
    }
}
