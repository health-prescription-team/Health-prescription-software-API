namespace Health_prescription_software_API.Models.Medicine
{
    using System.ComponentModel.DataAnnotations;

    public class EditMedicineDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public IFormFile MedicineImage { get; set; } = null!;

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Required]
        public string MedicineCompany { get; set; } = null!;

        [Required]
        public string MedicineDetails { get; set; } = null!;

        public string? Ingredients { get; set; }
    }
}
