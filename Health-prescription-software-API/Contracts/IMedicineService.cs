using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;

namespace Health_prescription_software_API.Contracts
{
    using Health_prescription_software_API.Data.Entities;

    public interface IMedicineService
    {

        Task<MedicineDetailsDTO?> GetById(int id);

        Task Add(AddMedicineDTO model);
      
        Task EditByIdAsync(int id, EditMedicineDTO editMedicineModel);

        Task<bool> Delete(int id);
    
        Task<AllMedicineDTO[]> GetAllAsync(QueryMedicineDTO? query);
	}

}
