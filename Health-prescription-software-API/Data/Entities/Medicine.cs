using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Data.Entities
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; } = null!;




        [Required] 

        public byte[]? MedicineImageBytes { get; set; }




        [Required]
        public decimal Price { get; set; }

        [Required] 
        public string MedicineCompany { get; set; } = null!;

        [Required] 
        public string MedicineDetails { get; set; } = null!;



    }
}
