namespace Health_prescription_software_API.Contracts
{

  using Data.Entities;
  using Models.Medicine;

    public interface IMedicineService
    {

        Task<MedicineDetailsDTO?> GetById(int id);

        Task Add(AddMedicineDTO model);
     
        Task EditByIdAsync(int id, EditMedicineDTO editMedicineModel);
      
        Task<AllMedicineServiceModel> GetAllAsync(QueryMedicineDTO? query);
      
        Task<bool> Delete(int id);
    }
}
