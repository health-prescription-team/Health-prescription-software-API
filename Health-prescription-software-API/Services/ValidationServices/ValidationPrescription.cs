namespace Health_prescription_software_API.Services.ValidationServices
{
    using Health_prescription_software_API.Contracts.Validations;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Models.Prescription;
    using Microsoft.EntityFrameworkCore;

    public class ValidationPrescription : IValidaitonPrescription
    {

        private readonly HealthPrescriptionDbContext dbContext;

        public ValidationPrescription(HealthPrescriptionDbContext healthPrescriptionDbContext)
        {
            dbContext = healthPrescriptionDbContext;
            ModelErrors =  new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsPrescriptionValid(AddPrescriptionDto prescriptionModel)
        {
            var patientExist = await dbContext.Users.FirstOrDefaultAsync(x => x.Egn == prescriptionModel.PatientEgn);

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

            foreach (var details in prescriptionModel.PrescriptionDetails)
            {
                var medicine = await dbContext.Medicines.FindAsync(details.MedicineId);

                if (medicine == null)
                {
                    var modelError = new ModelError
                    {
                        ErrorPropName = nameof(details.MedicineId),
                        ErrorMessage = $"Medicine with id: {details.MedicineId} does not exist."
                    };

                    ModelErrors.Add(modelError);

                    return false;
                }
            }

            return true;
        }
    }
}
