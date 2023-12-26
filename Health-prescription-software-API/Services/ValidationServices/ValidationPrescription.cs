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
                    ErrorPropName = nameof(prescriptionModel.PatientEgn),
                    ErrorMessage = PatientDoesNotExist

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

        public async Task<bool> IsDeletePrescriptionValid(Guid prescriptionId)
        {
            var prescription = await dbContext.Prescriptions.FindAsync(prescriptionId);

            if (prescription != null)
            {
                var modelError = new ModelError
                {
                    ErrorPropName = nameof(prescription.IsFulfilled),
                    ErrorMessage = CantDeletePrescription
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }

        public async Task<bool> IsEditPrescriptionValid(EditPrescriptionDTO model)
        {
            var prescription = await dbContext.Prescriptions.FindAsync(model.Id);

            if (prescription == null || prescription.IsFulfilled)
            {
                ModelError? modelError;

                if (prescription == null)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(prescription.Id),
                        ErrorMessage = PrescriptionDoesNotExist
                    };

                    ModelErrors.Add(modelError);
                }

                if (prescription!.IsFulfilled)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = nameof(prescription.IsFulfilled),
                        ErrorMessage = CantEditPrescription
                    };

                    ModelErrors.Add(modelError);
                }

                return false;
            }

            return true;
        }

        public async Task<bool> IsGpThePrescriber(string gpId, Guid prescriptionId)
        {
            var valid = await dbContext.Prescriptions.AnyAsync(p => p.Id == prescriptionId && p.GpId == gpId);

            if (!valid)
            {
                return false;
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
            var patientExist = await dbContext.Users.AnyAsync(u => u.Egn == patientEgn);

            if (!validEgn || !patientExist)
            {
                ModelError? modelError;

                if (!validEgn)
                {

                    modelError = new ModelError
                    {
                        ErrorPropName = "EGN",
                        ErrorMessage = InvalidEgnErrorMessage
                    };

                    ModelErrors.Add(modelError);
                }

                if (!patientExist)
                {
                    modelError = new ModelError
                    {
                        ErrorPropName = "EGN",
                        ErrorMessage = PatientDoesNotExist
                    };

                    ModelErrors.Add(modelError);
                }

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
                    ErrorPropName = "Id",
                    ErrorMessage = PrescriptionDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            return true;
        }
    }
}
