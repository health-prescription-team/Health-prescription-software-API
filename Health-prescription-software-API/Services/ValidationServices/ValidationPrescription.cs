using Health_prescription_software_API.Contracts.Validations;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Models.Prescription;
using Microsoft.EntityFrameworkCore;

namespace Health_prescription_software_API.Services.ValidationServices
{
    public class ValidationPrescription : IValidaitonPrescription
    {

        private readonly HealthPrescriptionDbContext _dbContext;

        public ValidationPrescription(HealthPrescriptionDbContext healthPrescriptionDbContext)
        {
            _dbContext = healthPrescriptionDbContext;
            ModelErrors =  new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsPrescriptionValid(AddPrescriptionDto prescriptionModel)
        {
            var patientExist = await _dbContext.Users.FirstOrDefaultAsync(x => x.Egn == prescriptionModel.Egn);

            if (patientExist == null)
            {
                var notFoundPatient = new ModelError
                {
                    ErrorMessage = "Patient cannot be found. He/She may not be registered",
                    ErrorPropName = nameof(patientExist.Egn)

                };

                ModelErrors.Add(notFoundPatient);

                return false;
            }

            return true;
        }
    }
}
