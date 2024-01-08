namespace Health_prescription_software_API.Services.ValidationServices
{
    using Microsoft.EntityFrameworkCore;
    using System.Text.RegularExpressions;

    using Data;
    using Models.Prescription;
    using Contracts.Validations;

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

            if (prescriptionModel.PrescriptionDetails.Count == 0)
            {
                var emptyDetails = new ModelError
                {
                    ErrorPropName = nameof(prescriptionModel.PrescriptionDetails),
                    ErrorMessage = PrescriptionDetailsAreRequired
                };

                ModelErrors.Add(emptyDetails);

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

            if (prescription == null)
            {
                var modelError = new ModelError
                {
                    ErrorPropName = nameof(prescription.Id),
                    ErrorMessage = PrescriptionDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            if (prescription.IsFulfilled)
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

            if (prescription == null)
            {
                var modelError = new ModelError
                {
                    ErrorPropName = nameof(prescription.Id),
                    ErrorMessage = PrescriptionDoesNotExist
                };

                ModelErrors.Add(modelError);

                return false;
            }

            if (prescription.IsFulfilled)
            {
                var modelError = new ModelError
                {
                    ErrorPropName = nameof(prescription.IsFulfilled),
                    ErrorMessage = CantEditPrescription
                };

                ModelErrors.Add(modelError);

                return false;
            }

            if (prescription.PrescriptionDetails.Count == 0)
            {
                var modelError = new ModelError
                {
                    ErrorPropName = nameof(prescription.PrescriptionDetails),
                    ErrorMessage = PrescriptionDetailsAreRequired
                };

                ModelErrors.Add(modelError);

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
