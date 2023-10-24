using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;

namespace Health_prescription_software_API.Contracts
{
    using Health_prescription_software_API.Data.Entities;

    public interface IMedicineService
    {

        Task<Medicine?> GetById(int id);

        void Add(AddMedicineDTO model);

    }
}
