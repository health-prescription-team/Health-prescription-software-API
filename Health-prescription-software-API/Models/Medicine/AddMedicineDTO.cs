using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Medicine
{
    public class AddMedicineDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required] public IFormFile MedicineFile { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string MedicineCompany { get; set; } = null!;

        [Required]
        public string MedicineDetails { get; set; } = null!;
    }
}
