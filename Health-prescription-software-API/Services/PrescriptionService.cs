﻿namespace Health_prescription_software_API.Services
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities;
    using Health_prescription_software_API.Models.Prescription;
    using Microsoft.EntityFrameworkCore;

    public class PrescriptionService : IPrescriptionService
    {
        private readonly HealthPrescriptionDbContext _context;

        public PrescriptionService(HealthPrescriptionDbContext context)
        {
            this._context = context;
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

            await _context.Prescriptions.AddAsync(prescriptionEntity);
            await _context.SaveChangesAsync();

            return prescriptionEntity.Id;
        }

        public void Delete(Guid id)
        {
            var modelDb = _context.Prescriptions.Find(id);

            _context.Prescriptions.Remove(modelDb!);
            _context.SaveChanges();
        }

        public async Task<Guid> Edit(EditPrescriptionDTO prescriptionModel, string GpId)
        {
            var entity = await _context.Prescriptions
                .Include(p => p.PrescriptionDetails)
                .FirstOrDefaultAsync(p => p.Id == prescriptionModel.Id);

            entity!.PatientEgn = prescriptionModel.PatientEgn;
            entity!.Age = prescriptionModel.Age;
            entity!.Diagnosis = prescriptionModel.Diagnosis;
            entity!.ExpiresAt = prescriptionModel.ExpiresAt;
            entity!.PrescriptionDetails = prescriptionModel.PrescriptionDetails
                .Select(pd => new PrescriptionDetails
                {
                    MedicineId = pd.MedicineId,
                    EveningDose = pd.EveningDose,
                    LunchTimeDose = pd.LunchTimeDose,
                    MorningDose = pd.MorningDose,
                    MeasurementUnit = pd.MeasurementUnit,
                    Notes = pd.Notes
                })
                .ToHashSet();

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> FinishPrescription(Guid id)
        {
            var prescriptionsList = await _context.Prescriptions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (prescriptionsList is null)
            {
                return false;
            }

            prescriptionsList.FulfillmentDate = DateTime.Now;
            prescriptionsList.IsFulfilled = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PatientPrescriptionsDTO> GetPatientPrescriptions(string patientEgn)
        {
            var patient = await _context.Users.FirstOrDefaultAsync(p => p.Egn == patientEgn);

            var prescriptionsList = await _context.Prescriptions
                .Where(p => p.PatientEgn == patientEgn)
                .Select(p => new PatientPrescriptionsListDTO
                {
                    PrescriptionId = p.Id,
                    CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd"),
                    ExpiresAt = p.ExpiresAt.HasValue ? p.ExpiresAt.Value.ToString("yyyy-MM-dd") : null,
                    IsFulfilled = p.IsFulfilled,
                    Medicaments = p.PrescriptionDetails
                        .Select(pd => pd.Medicine.Name)
                        .ToList()
                })
                .ToListAsync();

            var patientPrescriptions = new PatientPrescriptionsDTO
            {
                PatientEGN = patientEgn,
                PatientNames = $"{patient!.FirstName} {(string.IsNullOrEmpty(patient.MiddleName) ? "" : patient.MiddleName + " ")}{patient.LastName}",
                ProfileImage = patient.ProfilePicture,
                PatientPrescriptions = prescriptionsList
            };

            return patientPrescriptions;
        }

        public async Task<PrescriptionDTO> GetPrescriptionDetails(Guid prescriptionId)
        {
            var entity = await _context.Prescriptions
                .Include(p => p.Gp)
                .Include(p => p.PrescriptionDetails)
                .ThenInclude(pd => pd.Medicine)
                .FirstOrDefaultAsync(p => p.Id == prescriptionId);

            var gpFullName = $"{entity!.Gp.FirstName} {(string.IsNullOrEmpty(entity.Gp.MiddleName) ? "" : entity.Gp.MiddleName + " ")}{entity.Gp.LastName}";

            PrescriptionDTO prescription = new()
            {
                PatientEgn = entity!.PatientEgn,
                GpFullName = gpFullName,
                GpEgn = entity.Gp.Egn!,
                Age = entity.Age,
                Diagnosis = entity.Diagnosis,
                IsFulfilled = entity.IsFulfilled,
                FulfillmentDate = entity.FulfillmentDate.HasValue ? entity.FulfillmentDate.Value.ToString("yyyy-MM-dd") : null,
                CreatedAt = entity.CreatedAt.ToString("yyyy-MM-dd"),
                ExpiresAt = entity.ExpiresAt.HasValue ? entity.ExpiresAt.Value.ToString("yyyy-MM-dd") : null,
                PrescriptionDetails = entity.PrescriptionDetails
                    .Select(pd => new PrescriptionDetailsDTO
                    {
                        MedicineId = pd.MedicineId,
                        MedicineName = pd.Medicine.Name,
                        Notes = pd.Notes,
                        EveningDose = pd.EveningDose,
                        LunchTimeDose = pd.LunchTimeDose,
                        MorningDose = pd.MorningDose,
                        MeasurementUnit = pd.MeasurementUnit
                    })
                    .ToHashSet()
            };

            return prescription;
        }
    }
}
