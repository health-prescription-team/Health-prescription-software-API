namespace Health_prescription_software_API.Contracts
{
    using Models.Medicine;
    public interface IMedicineService
    {

        Task<MedicineDetailsDTO?> GetById(int id);

        Task Add(AddMedicineDTO model);

        Task EditByIdAsync(int id, EditMedicineDTO editMedicineModel);

        Task<AllMedicineServiceModel> GetAllAsync(QueryMedicineDTO? query);

        Task<IEnumerable<AllMedicineMinimalDTO>> GetAllMinimalAsync();

        Task<bool> Delete(int id);
    }
}
