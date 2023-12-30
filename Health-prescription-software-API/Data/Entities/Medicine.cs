namespace Health_prescription_software_API.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Medicine
    {
        [Key]
        public Guid Id { get; set; }

        [Required] 
        public string Name { get; set; } = null!;

        [Required] 
        public byte[]? MedicineImageBytes { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string OwnerId { get; set; } = null!;
        [ForeignKey(nameof(OwnerId))]
        public virtual User.User Owner { get; set; } = null!;

        [Required] 
        public string MedicineCompany { get; set; } = null!;

        [Required] 
        public string MedicineDetails { get; set; } = null!;

        public string? Ingredients { get; set; }
    }
}
