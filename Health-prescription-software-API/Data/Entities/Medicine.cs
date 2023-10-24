using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Data.Entities
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
