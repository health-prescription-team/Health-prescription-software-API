namespace Health_prescription_software_API.Services.ValidationServices
{
    using Health_prescription_software_API.Contracts.Validations;
    using Health_prescription_software_API.Data;
	using Health_prescription_software_API.Models.Authentication.Pharmacy;
	using System.Threading.Tasks;

    public class ValidationAuthentication : IValidationAuthentication
	{
		private readonly HealthPrescriptionDbContext dbContext;

		public ValidationAuthentication(HealthPrescriptionDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public string? ErrorMessage { get; set; }

		public string? ErrorPropName { get; set; }

		public Task<bool> IsPharmacyRegisterValid(RegisterPharmacyDto registerModel)
		{
			throw new NotImplementedException();
		}
	}
}
