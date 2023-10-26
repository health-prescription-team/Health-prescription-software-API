using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;

namespace Health_prescription_software_API.Contracts
{

  using Data.Entities;
  using Models.Medicine;

    public interface IMedicineService
    {

        Task<MedicineDetailsDTO?> GetById(int id);

        Task Add(AddMedicineDTO model);
     
        Task EditByIdAsync(int id, EditMedicineDTO editMedicineModel);
      
        Task<AllMedicineDTO[]> GetAllAsync(QueryMedicineDTO? query);
      
        Task<bool> Delete(int id);
    }
}
