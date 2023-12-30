namespace Health_prescription_software_API.Contracts
{
    using Models.Medicine;
    public interface IMedicineService
    {

        Task<MedicineDetailsDTO?> GetById(Guid id);

        Task<Guid> Add(AddMedicineDTO model, string creatorId);

        Task EditByIdAsync(Guid id, EditMedicineDTO editMedicineModel);

        Task<AllMedicineServiceModel> GetAllAsync(QueryMedicineDTO? query);

        Task<IEnumerable<AllMedicineMinimalDTO>> GetAllMinimalAsync();

        Task<bool> Delete(Guid id);
    }
}
