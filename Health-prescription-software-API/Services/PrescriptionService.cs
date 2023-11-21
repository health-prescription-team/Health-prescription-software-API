using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Prescription;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Health_prescription_software_API.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly HealthPrescriptionDbContext _context;

        public PrescriptionService(HealthPrescriptionDbContext context)
        {
            this._context = context;
        }


        public async Task<int> Add(AddPrescriptionDto prescriptionModel)
        {

            if ( await _context.Users.FirstOrDefaultAsync(x => x.Egn == prescriptionModel.Egn) is null)
            {
                throw new NullReferenceException("Ne e nameren chovek s tova egn");
            }


            var modelDb = new Prescription
            {
                PatientId = prescriptionModel.PatientId,
                Age = prescriptionModel.Age,
                GpName = prescriptionModel.GpName,
                Diagnosis = prescriptionModel.Diagnosis,
                CreatedAt = prescriptionModel.CreatedAt,
                EndedAt = prescriptionModel.EndedAt,
                Egn = prescriptionModel.Egn,
                IsActive = true,
                

            };

          _context.Prescriptions.Add(modelDb);
          await  _context.SaveChangesAsync();

            return modelDb.Id;
             
        }


       
    }
}
