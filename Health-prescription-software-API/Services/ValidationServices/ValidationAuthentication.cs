namespace Health_prescription_software_API.Services.ValidationServices
{
    using Health_prescription_software_API.Contracts.Validations;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Models.Authentication.Pharmacy;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ValidationAuthentication : IValidationAuthentication
    {
        private readonly HealthPrescriptionDbContext dbContext;

        public ValidationAuthentication(HealthPrescriptionDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.ModelErrors = new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsPharmacyRegisterValid(RegisterPharmacyDto registerModel)
        {
            bool isEmailPresent = await dbContext.Users
                .AnyAsync(u => u.Email == registerModel.Email);
            if (isEmailPresent)
            {
                var error = new ModelError
                {
                    ErrorPropName = nameof(registerModel.Email),
                    ErrorMessage = "User with the same email already exists."
                };

                ModelErrors.Add(error);

                return false;
            }
            // todo: more checking if needed

            return true;
        }
    }
}
