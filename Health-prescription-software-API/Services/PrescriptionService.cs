﻿namespace Health_prescription_software_API.Services
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities;
    using Health_prescription_software_API.Models.Prescription;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Cryptography;

    public class PrescriptionService : IPrescriptionService
    {
        private readonly HealthPrescriptionDbContext context;

        public PrescriptionService(HealthPrescriptionDbContext context)
        {
            this.context = context;
        }

        public async Task<string> Add(AddPrescriptionDto prescriptionModel, string GpId)
        {
            var patient = await context.Users.FirstOrDefaultAsync(x => x.Egn == prescriptionModel.PatientEgn);

            var prescriptionEntity = new Prescription
            {
                Age = prescriptionModel.Age,
                GpId = GpId,
                Diagnosis = prescriptionModel.Diagnosis,
                CreatedAt = DateTime.Now,
                ExpiresAt = prescriptionModel.ExpiresAt,
                PatientEgn = prescriptionModel.PatientEgn
            };

            context.Prescriptions.Add(prescriptionEntity);
            await context.SaveChangesAsync();

            foreach (var details in prescriptionModel.PrescriptionDetails)
            {
                var prescriptionDetails = new PrescriptionDetails
                {
                    MedicineId = details.MedicineId,
                    PrescriptionId = prescriptionEntity.Id,
                    EveningDose = details.EveningDose,
                    LunchTimeDose = details.LunchTimeDose,
                    MorningDose = details.MorningDose,
                    MeasurementUnit = details.MeasurementUnit,
                    Notes = details.Notes
                };

                context.PrescriptionDetails.Add(prescriptionDetails);
            }

            await context.SaveChangesAsync();

            var hashedIdString = HashingAlgorithm(prescriptionEntity.Id);

            return hashedIdString;
        }

        private static string HashingAlgorithm(int value)
        {
            byte[] idPrescriptionToBytes = BitConverter.GetBytes(value);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(idPrescriptionToBytes);


                string hashedValue = BitConverter.ToString(hashBytes).Replace("-", "");

                return hashedValue;
            }
        }

    }
}
