namespace Health_prescription_software_API.Services
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities;
    using Health_prescription_software_API.Models.Prescription;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class PrescriptionService : IPrescriptionService
    {
        private readonly HealthPrescriptionDbContext context;

        public PrescriptionService(HealthPrescriptionDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Add(AddPrescriptionDto prescriptionModel, string GpId)
        {
            var prescriptionEntity = new Prescription
            {
                Age = prescriptionModel.Age,
                GpId = GpId,
                Diagnosis = prescriptionModel.Diagnosis,
                CreatedAt = DateTime.Now,
                ExpiresAt = prescriptionModel.ExpiresAt,
                PatientEgn = prescriptionModel.PatientEgn
            };

            foreach (var details in prescriptionModel.PrescriptionDetails)
            {
                prescriptionEntity.PrescriptionDetails.Add(new PrescriptionDetails
                {
                    MedicineId = details.MedicineId,
                    PrescriptionId = prescriptionEntity.Id,
                    EveningDose = details.EveningDose,
                    LunchTimeDose = details.LunchTimeDose,
                    MorningDose = details.MorningDose,
                    MeasurementUnit = details.MeasurementUnit,
                    Notes = details.Notes
                });
            }

            context.Prescriptions.Add(prescriptionEntity);
            await context.SaveChangesAsync();

            return prescriptionEntity.Id;
        }

        public async Task<IEnumerable<PatientPrescriptionsListDTO>> GetPatientPrescriptions(string patientId)
        {
            var patient = await context.Users.FindAsync(patientId);

            var prescriptionsList = await context.Prescriptions
                .Where(p => p.PatientEgn == patient!.Egn)
                .Select(p => new PatientPrescriptionsListDTO
                {
                    PrescriptionId = p.Id,
                    CreatedAt = p.CreatedAt,
                    ExpiresAt = p.ExpiresAt,
                    IsFulfilled = p.IsFulfilled,
                    Medicaments = p.PrescriptionDetails
                        .Select(pd => pd.Medicine.Name)
                        .ToList()
                })
                .ToListAsync();

            return prescriptionsList;
        }
    }
}
