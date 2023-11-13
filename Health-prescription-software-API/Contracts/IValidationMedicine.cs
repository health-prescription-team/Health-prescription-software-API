namespace Health_prescription_software_API.Contracts
{
	using Health_prescription_software_API.Models.Medicine;

	public interface IValidationMedicine
	{

		public string? ErrorMessage { get;}
		public string? ErrorPropName { get;}

		Task<bool> IsQueryValid(QueryMedicineDTO? queryModel);
	}
}
