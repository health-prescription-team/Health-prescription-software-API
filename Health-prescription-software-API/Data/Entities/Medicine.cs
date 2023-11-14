using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Data.Entities
{
    public class Medicine
    {
        public Medicine()
        {
			UsersMedicines = new HashSet<UserMedicine>();
        }

        [Key]
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; } = null!;

        [Required] 

        public byte[]? MedicineImageBytes { get; set; }

        [Required]
        public decimal Price { get; set; } //average

        [Required] 
        public string MedicineCompany { get; set; } = null!;

        [Required] 
        public string MedicineDetails { get; set; } = null!;

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public ICollection<UserMedicine> UsersMedicines { get; set; }
    }
}
