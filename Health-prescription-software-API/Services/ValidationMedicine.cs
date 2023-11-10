namespace Health_prescription_software_API.Services
{
	using Health_prescription_software_API.Contracts;
	using Health_prescription_software_API.Models.Medicine;

	public class ValidationMedicine : IValidationMedicine
	{
		public string? ErrorMessage => throw new NotImplementedException();

		public bool IsQueryValide(QueryMedicineDTO? queryModel)
		{
			throw new NotImplementedException();
		}
	}
}
