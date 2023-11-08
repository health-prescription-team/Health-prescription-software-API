namespace Health_prescription_software_API.Contracts
{
	using Health_prescription_software_API.Models.Medicine;

	public interface IValidationMedicine
	{

        public string? ErrorMessage { get; }

		bool IsQueryValide(QueryMedicineDTO? queryModel);
	}
}
