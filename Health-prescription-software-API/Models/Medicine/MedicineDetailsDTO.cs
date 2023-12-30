namespace Health_prescription_software_API.Models.Medicine
{
    public class MedicineDetailsDTO
    {
        public string Name { get; set; } = null!;

        public byte[]? MedicineImageBytes { get; set; }

        public decimal Price { get; set; }

        public string MedicineCompany { get; set; } = null!;

        public string MedicineDetails { get; set; } = null!;

        public string? Ingredients { get; set; }
    }
}
