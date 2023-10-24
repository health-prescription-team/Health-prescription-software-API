using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;

namespace Health_prescription_software_API.Contracts
{
    public interface IMedicineService
    {
        void Add(AddMedicineDTO model);
    }
}
