using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Prescription;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Health_prescription_software_API.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly HealthPrescriptionDbContext _context;

        public PrescriptionService(HealthPrescriptionDbContext context)
        {
            this._context = context;
        }


        public async Task<string> Add(AddPrescriptionDto prescriptionModel)
        {

           var patient = await _context.Users.FirstOrDefaultAsync(x => x.Egn == prescriptionModel.Egn);

            var modelDb = new Prescription
            {
                PatientId = patient.Id,
                Age = prescriptionModel.Age,
                GpName = prescriptionModel.GpName,
                Diagnosis = prescriptionModel.Diagnosis,
                CreatedAt = DateTime.Now,
                EndedAt = prescriptionModel.EndedAt,
                Egn = prescriptionModel.Egn,
                IsActive = true,

            };

          await _context.Prescriptions.AddAsync(modelDb);
          await  _context.SaveChangesAsync();

           
            var hashedIdString = await HashingAlgorithm(modelDb.Id);

            return hashedIdString;
        }




        private async Task<string> HashingAlgorithm(int value)
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
