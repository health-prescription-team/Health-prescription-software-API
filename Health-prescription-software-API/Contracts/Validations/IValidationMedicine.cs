namespace Health_prescription_software_API.Contracts.Validations
{
    using Health_prescription_software_API.Models.Medicine;

    public interface IValidationMedicine : IValidationErrorMessage
    {
        Task<bool> IsMedicineValid(Guid id);

        Task<bool> IsQueryValid(QueryMedicineDTO? queryModel);
    }
}
