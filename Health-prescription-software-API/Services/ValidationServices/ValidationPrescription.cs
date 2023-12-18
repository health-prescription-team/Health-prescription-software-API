namespace Health_prescription_software_API.Services.ValidationServices
{
    using Health_prescription_software_API.Contracts.Validations;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Models.Prescription;
    using Microsoft.EntityFrameworkCore;
    using System.Text.RegularExpressions;
    using static Common.EntityValidationErrorMessages.Medicine;
    using static Common.EntityValidationErrorMessages.Prescription;
    using static Common.EntityValidationConstants.User;
    using System;

    public class ValidationPrescription : IValidationPrescription
    {

        private readonly HealthPrescriptionDbContext dbContext;

        public ValidationPrescription(HealthPrescriptionDbContext healthPrescriptionDbContext)
        {
            dbContext = healthPrescriptionDbContext;
            ModelErrors = new HashSet<ModelError>();
        }

        public ICollection<ModelError> ModelErrors { get; set; }

        public async Task<bool> IsAddPrescriptionValid(AddPrescriptionDto prescriptionModel)
        {
            var patientExist = await dbContext.Users.AnyAsync(x => x.Egn == prescriptionModel.PatientEgn);

            if (!patientExist)
            {
                var notFoundPatient = new ModelError
                {
                    ErrorMessage = PatientDoesNotExist,
                    ErrorPropName = nameof(prescriptionModel.PatientEgn)

                };

                ModelErrors.Add(notFoundPatient);

                return false;
            }

            foreach (var details in prescriptionModel.PrescriptionDetails)
            {
                var medicineExists = await dbContext.Medicines.AnyAsync(m => m.Id == details.MedicineId);

                if (!medicineExists)
                {
                    var modelError = new ModelError
                    {
                        ErrorPropName = nameof(details.MedicineId),
                        ErrorMessage = InvalidMedicineId
                    };

                    ModelErrors.Add(modelError);

                    return false;
                }
            }

            return true;
        }

        public async Task<bool> IsPatientPrescriptionsValid(string patientEgn)
        {
            if (string.IsNullOrWhiteSpace(patientEgn))
            {
                return false;
            }

            var validEgn = Regex.Match(patientEgn, EgnRegexPattern).Success;

            if (!validEgn)
            {
                var invalidEgn = new ModelError
                {
                    ErrorPropName = "EGN",
                    ErrorMessage = InvalidEgnErrorMessage
                };

                ModelErrors.Add(invalidEgn);

                return false;
            }

            var patientExist = await dbContext.Users.AnyAsync(u => u.Egn == patientEgn);

            if (!patientExist)
            {
                var notFoundPatient = new ModelError
                {
                    ErrorMessage = PatientDoesNotExist,
                    ErrorPropName = "EGN"

                };

                ModelErrors.Add(notFoundPatient);

                return false;
            }

            return true;
        }

        public async Task<bool> IsPrescriptionValid(Guid prescriptionId)
        {
            var validId = await dbContext.Prescriptions.AnyAsync(p => p.Id == prescriptionId);

            if (!validId)
            {
                var modelError = new ModelError
                {
                    ErrorMessage = PrescriptionDoesNotExist,
                    ErrorPropName = "Id"
                };

                return false;
            }

            return true;
        }
    }
}
